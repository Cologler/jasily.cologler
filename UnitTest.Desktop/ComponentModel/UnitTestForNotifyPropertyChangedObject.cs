using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;
using Jasily.ComponentModel;

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
            Assert.IsTrue(instance.LastChangedPropertyName.Count == 0);
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
                this.LastChangedPropertyName = new List<string>();

                this.PropertyChanged += this.NotifyPropertyChangedObjectInstance_PropertyChanged;
            }

            void NotifyPropertyChangedObjectInstance_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                this.LastChangedPropertyName.Add(e.PropertyName);
            }

            public string Name
            {
                get { return this._name; }
                set { this.SetPropertyRef(ref this._name, value); }
            }
        }
    }
}
