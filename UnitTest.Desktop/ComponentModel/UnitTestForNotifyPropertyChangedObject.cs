using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;

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

            instance._name = "6";
            instance.RegisterForEndRefresh<NotifyPropertyChangedObjectInstance>(z => z.Name);
            instance.EndRefresh();
            Assert.IsTrue(instance.LastChangedPropertyName.Count == 1);
            Assert.IsTrue(instance.LastChangedPropertyName[0] == "Name");
        }

        public class NotifyPropertyChangedObjectInstance : NotifyPropertyChangedObject
        {
            public List<string> LastChangedPropertyName;

            public string _name;

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
