using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows;
using System.Security;
using System.Runtime.InteropServices;
using System.ComponentModel;

// ========================
// source from http://blogs.microsoft.co.il/blogs/arik/SingleInstance.cs.txt
// modified by cologler.
// alse see: http://codereview.stackexchange.com/questions/20871/single-instance-wpf-application
// ------------
// HOW TO USE
// ------------
// class App : Application, ISingleInstanceApp
// {
//     protected override void OnStartup(StartupEventArgs e)
//     {
//         using (var p = new MicrosoftSingleInstanceProvider<App>(this))
//         {
//             if (p.InitializeAsFirstInstance("{3DD4E9C7-4084-4E46-A0FC-F8B4CD0D55A4}"))
//             {
//                 // current is first instance
//             }
//             else
//             {
//                 this.Shutdown();
//             }
//         }
//     }
//     
//     protected override void OnExit(ExitEventArgs e)
//     {
//         if (p != null)
//         {
//             p.Dispose();
//             p = null;
//         }
//     }
//     

namespace System.Shell
{
    public class MicrosoftSingleInstanceProvider<TApplication> : MicrosoftSingleInstanceProvider
        where TApplication : Application, ISingleInstanceApp
    {
        TApplication App;

        public MicrosoftSingleInstanceProvider(TApplication app)
        {
            this.App = app.ThrowIfNull("app");
        }

        public override void Callback(string[] args)
        {
 	         base.Callback(args);

             this.App.SignalExternalCommandLineArgs(args);
        }
    }

    /// <summary>
    /// This class checks to make sure that only one instance of 
    /// this application is running at a time.
    /// </summary>
    /// <remarks>
    /// Note: this class should be used with some caution, because it does no
    /// security checking. For example, if one instance of an app that uses this class
    /// is running as Administrator, any other instance, even if it is not
    /// running as Administrator, can activate it with command line arguments.
    /// For most apps, this will not be much of an issue.
    /// </remarks>
    public class MicrosoftSingleInstanceProvider : MarshalByRefObject, IDisposable
    {
        #region Private Fields

        /// <summary>
        /// String delimiter used in channel names.
        /// </summary>
        private const string Delimiter = ":";

        /// <summary>
        /// Suffix to the channel name.
        /// </summary>
        private const string ChannelNameSuffix = "SingeInstanceIPCChannel";

        /// <summary>
        /// Remote service name.
        /// </summary>
        private const string RemoteServiceName = "SingleInstanceApplicationService";

        /// <summary>
        /// IPC protocol used (string).
        /// </summary>
        private const string IpcProtocol = "ipc://";

        /// <summary>
        /// Application mutex.
        /// </summary>
        private Mutex singleInstanceMutex;

        /// <summary>
        /// IPC channel for communications.
        /// </summary>
        private IpcServerChannel channel;

        /// <summary>
        /// List of command line arguments for the application.
        /// </summary>
        private string[] commandLineArgs;

        #endregion

        public virtual void Callback(string[] args)
        {

        }

        #region Public Methods

        /// <summary>
        /// Checks if the instance of the application attempting to start is the first instance. 
        /// If not, activates the first instance.
        /// </summary>
        /// <returns>True if this is the first instance of the application.</returns>
        public bool InitializeAsFirstInstance(string uniqueName)
        {
            commandLineArgs = GetCommandLineArgs(uniqueName);

            // Build unique application Id and the IPC channel name.
            var applicationIdentifier = uniqueName + Environment.UserName;

            var channelName = String.Concat(applicationIdentifier, Delimiter, ChannelNameSuffix);

            // Create mutex based on unique application Id to check if this is the first instance of the application. 
            bool firstInstance;
            singleInstanceMutex = new Mutex(true, applicationIdentifier, out firstInstance);
            if (firstInstance)
            {
                CreateRemoteService(channelName);
            }
            else
            {
                SignalFirstInstance(channelName, commandLineArgs);
            }

            return firstInstance;
        }

        /// <summary>
        /// Cleans up single-instance code, clearing shared resources, mutexes, etc.
        /// </summary>
        public void Dispose()
        {
            if (singleInstanceMutex != null)
            {
                singleInstanceMutex.Close();
                singleInstanceMutex = null;
            }

            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets command line args - for ClickOnce deployed applications, command line args may not be passed directly, they have to be retrieved.
        /// </summary>
        /// <returns>List of command line arg strings.</returns>
        private static string[] GetCommandLineArgs(string uniqueApplicationName)
        {
            string[] args = null;
            if (AppDomain.CurrentDomain.ActivationContext == null)
            {
                // The application was not clickonce deployed, get args from standard API's
                args = Environment.GetCommandLineArgs();
            }
            else
            {
                // The application was clickonce deployed
                // Clickonce deployed apps cannot recieve traditional commandline arguments
                // As a workaround commandline arguments can be written to a shared location before 
                // the app is launched and the app can obtain its commandline arguments from the 
                // shared location               
                var appFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), uniqueApplicationName);

                var cmdLinePath = Path.Combine(appFolderPath, "cmdline.txt");
                if (File.Exists(cmdLinePath))
                {
                    try
                    {
                        using (TextReader reader = new StreamReader(cmdLinePath, System.Text.Encoding.Unicode))
                        {
                            args = NativeMethods.CommandLineToArgvW(reader.ReadToEnd());
                        }

                        File.Delete(cmdLinePath);
                    }
                    catch (IOException)
                    {
                    }
                }
            }

            if (args == null)
            {
                args = new string[] { };
            }

            return args;
        }

        /// <summary>
        /// Creates a remote service for communication.
        /// </summary>
        /// <param name="channelName">Application's IPC channel name.</param>
        private void CreateRemoteService(string channelName)
        {
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Dictionary<string, string>();

            props["name"] = channelName;
            props["portName"] = channelName;
            props["exclusiveAddressUse"] = "false";

            // Create the IPC Server channel with the channel properties
            channel = new IpcServerChannel(props, serverProvider);

            // Register the channel with the channel services
            ChannelServices.RegisterChannel(channel, true);

            // Expose the remote service with the REMOTE_SERVICE_NAME
            RemotingServices.Marshal(this, RemoteServiceName);
        }

        /// <summary>
        /// Creates a client channel and obtains a reference to the remoting service exposed by the server - 
        /// in this case, the remoting service exposed by the first instance. Calls a function of the remoting service 
        /// class to pass on command line arguments from the second instance to the first and cause it to activate itself.
        /// </summary>
        /// <param name="channelName">Application's IPC channel name.</param>
        /// <param name="args">
        /// Command line arguments for the second instance, passed to the first instance to take appropriate action.
        /// </param>
        private static void SignalFirstInstance(string channelName, string[] args)
        {
            var secondInstanceChannel = new IpcClientChannel();
            ChannelServices.RegisterChannel(secondInstanceChannel, true);

            var remotingServiceUrl = IpcProtocol + channelName + "/" + RemoteServiceName;

            // Obtain a reference to the remoting service exposed by the server i.e the first instance of the application
            var firstInstanceRemoteServiceReference = (MicrosoftSingleInstanceProvider)
                RemotingServices.Connect(typeof(MicrosoftSingleInstanceProvider), remotingServiceUrl);

            // Check that the remote service exists, in some cases the first instance may not yet have created one, in which case
            // the second instance should just exit
            if (firstInstanceRemoteServiceReference != null)
            {
                // Invoke a method of the remote service exposed by the first instance passing on the command line
                // arguments and causing the first instance to activate itself
                firstInstanceRemoteServiceReference.Callback(args);
            }
        }

        /// <summary>
        /// Remoting Object's ease expires after every 5 minutes by default. We need to override the InitializeLifetimeService class
        /// to ensure that lease never expires.
        /// </summary>
        /// <returns>Always null.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion

        [SuppressUnmanagedCodeSecurity]
        internal static class NativeMethods
        {
            [DllImport("shell32.dll", EntryPoint = "CommandLineToArgvW", CharSet = CharSet.Unicode)]
            private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);


            [DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
            private static extern IntPtr _LocalFree(IntPtr hMem);

            public static string[] CommandLineToArgvW(string cmdLine)
            {
                var argv = IntPtr.Zero;
                try
                {
                    var numArgs = 0;

                    argv = _CommandLineToArgvW(cmdLine, out numArgs);
                    if (argv == IntPtr.Zero)
                    {
                        throw new Win32Exception();
                    }
                    var result = new string[numArgs];

                    for (var i = 0; i < numArgs; i++)
                    {
                        var currArg = Marshal.ReadIntPtr(argv, i * Marshal.SizeOf(typeof(IntPtr)));
                        result[i] = Marshal.PtrToStringUni(currArg);
                    }

                    return result;
                }
                finally
                {

                    var p = _LocalFree(argv);
                    // Otherwise LocalFree failed.
                    // Assert.AreEqual(IntPtr.Zero, p);
                }
            }

        }
    }
}
