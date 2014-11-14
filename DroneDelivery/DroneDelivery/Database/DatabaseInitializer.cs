using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace DroneDelivery.Database
{
    /// <summary>
    /// For testing purposes / demo creation and to work within the time alloted, this initializer automatically
    /// regenerates the database if there's any changes.
    /// </summary>
#if DEBUG
    public class DatabaseInitializer : System.Data.Entity.DropCreateDatabaseAlways<DatabaseContext>
#else
    public class DatabaseInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>
#endif
    {
        protected DatabaseContext db;

        /// <summary>
        /// quick pseudo-deserialization of xml into custom object for the express purpose of seeding the DB.
        /// </summary>
        public void XmlToType<T>(ref T obj, XElement el)
        {
            var type = Activator.CreateInstance<T>().GetType();
            foreach (var node in el.Descendants())
            {
                PropertyInfo prop;
                string name = node.Name.ToString();

                if ((prop = type.GetProperty(name, typeof(string))) != null)
                {
                    prop.SetValue(obj, node.Value);
                    continue;
                }

                if ((prop = type.GetProperty(name, typeof(double))) != null)
                {
                    prop.SetValue(obj, double.Parse(node.Value));
                    continue;
                }

                if ((prop = type.GetProperty(name, typeof(int))) != null)
                {
                    prop.SetValue(obj, int.Parse(node.Value));
                    continue;
                }

                if ((prop = type.GetProperty(name, typeof(bool))) != null)
                {
                    prop.SetValue(obj, bool.Parse(node.Value));
                    continue;
                }
            }
        }

        public XDocument LoadXMLResource(string name)
        {
            XDocument result;

            name = "DroneDelivery.Sample_Data." + name;

            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(name))
            {
                using (TextReader tr = new StreamReader(stream))
                {
                    result = XDocument.Parse(tr.ReadToEnd());
                }
            }

            return result;
        }

        public void CreateProducts()
        {
            var products = LoadXMLResource("Products.xml");
            foreach (var p in products.Descendants(XName.Get("product")))
            {
                var product = db.Products.Create();
                XmlToType<Models.Product>(ref product, p);
                db.Products.Add(product);
            }

            db.SaveChanges();
        }

        protected override void Seed(DatabaseContext context)
        {
            db = context;
            CreateProducts();
        }

    }
}