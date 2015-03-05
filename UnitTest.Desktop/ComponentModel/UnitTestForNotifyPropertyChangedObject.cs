using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Desktop.ComponentModel
{
    [TestClass]
    public class UnitTestForNotifyPropertyChangedObject
    {
        [TestMethod]
        public void TestForSetPropertyRef()
        {
            var instance = new NotifyPropertyChangedObjectInstance();

            Assert.IsNull(instance.Name);
            Assert.IsTrue(instance.LastChangedPropertyName.Count == 0);

            instance.Name = null;
            Assert.IsNull(instance.Name);
            Assert.IsTrue(instance.LastChangedPropertyName.Count == 0);

            instance.Name = "1";
            Assert.AreEqual(instance.Name, "1");
            Assert.IsTrue(instance.LastChangedPropertyName.Count == 1);
            Assert.IsTrue(instance.LastChangedPropertyName[0] == "Name");
            instance.LastChangedPropertyName.Clear();

            instance.Name = "1";
            Assert.AreEqual(instance.Name, "1");
            Assert.IsTrue(instance.LastChangedPropertyName.Count == 0);
        }

        public class NotifyPropertyChangedObjectInstance : NotifyPropertyChangedObject
        {
            public List<string> LastChangedPropertyName;

            private string _name;

            public NotifyPropertyChangedObjectInstance()
            {
                LastChangedPropertyName = new List<string>();

                this.PropertyChanged += NotifyPropertyChangedObjectInstance_PropertyChanged;
            }

            void NotifyPropertyChangedObjectInstance_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                LastChangedPropertyName.Add(e.PropertyName);
            }

            public string Name
            {
                get { return _name; }
                set { SetPropertyRef(ref _name, value); }
            }
        }
    }
}
