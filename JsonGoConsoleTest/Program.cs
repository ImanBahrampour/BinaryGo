﻿using JsonGo;
using JsonGo.Deserialize;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonGoConsoleTest
{



    public class Product
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Profile Profile { get; set; }
        public List<Address> Addresses { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class Profile
    {
        public string FullName { get; set; }
        public List<Address> Addresses { get; set; }
    }

    public class Address
    {
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public AddressType Type { get; set; }
        public Profile Parent { get; set; }
    }

    public enum AddressType : byte
    {
        None = 0,
        Home = 1,
        Work = 2
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Enter number of items: ");
                int CYCLES = int.Parse(Console.ReadLine());



                decimal JsonGoRes;
                decimal JsonNetRes;

                List<List<Product>> fullProducts = new List<List<Product>>();
                List<Product> products = new List<Product>();
                Product product = new Product()
                {
                    Age = 27,
                    Name = "gl502vm",
                    Profile = new Profile()
                    {
                        FullName = "ali yousefi",
                        Addresses = new List<Address>()
                        {
                            new Address ()
                            {
                                 Content = "آدرس تلور",
                                 CreatedDate = DateTime.Now,
                                 Type = AddressType.Home
                            },
                            new Address ()
                            {
                                 Content = "آدرس مشهد خیابان علی عصر",
                                 CreatedDate = DateTime.Now.AddYears(1),
                                 Type = AddressType.Work
                            }
                        }
                    },
                    IsEnabled = true
                };
                Product product2 = new Product()
                {
                    Age = 22,
                    Name = "asus rog",
                    Profile = new Profile()
                    {
                        FullName = "amin mardani",
                        Addresses = new List<Address>()
                        {
                            new Address ()
                            {
                                 Content = "آدرس نوکنده",
                                 CreatedDate = DateTime.Now,
                                 Type = AddressType.Home
                            },
                            new Address ()
                            {
                                 Content = "آدرس جنگل گلستان",
                                 CreatedDate = DateTime.Now.AddYears(1),
                                 Type = AddressType.Work
                            }
                        }
                    },
                    IsEnabled = false
                };

                foreach (Address item in product.Profile.Addresses)
                {
                    item.Parent = product.Profile;
                }
                product.Profile.Addresses.AddRange(product.Profile.Addresses);
                product.Addresses = product.Profile.Addresses;
                products.Add(product);
                products.Add(product);
                products.Add(product2);

                fullProducts.Add(products);
                fullProducts.Add(products);
                Serializer serializer = new Serializer();
                string newtonJson = Newtonsoft.Json.JsonConvert.SerializeObject(fullProducts, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                string jsonGoJson = serializer.Serialize(fullProducts);


                Console.WriteLine($"***  SERIALIZE {CYCLES} items ***");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();



                // JSONNET serialize
                for (int i = 0; i < CYCLES; i++)
                {
                    string js = Newtonsoft.Json.JsonConvert.SerializeObject(fullProducts, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                stopwatch.Stop();
                JsonNetRes = stopwatch.ElapsedTicks;

                Console.WriteLine("Newtonsoft.Json: \t " + stopwatch.Elapsed);
                stopwatch.Reset();

                //JSONGO SERIALIZE
                stopwatch.Start();
                for (int i = 0; i < CYCLES; i++)
                {
                    string js = serializer.Serialize(fullProducts);
                }
                stopwatch.Stop();
                JsonGoRes = stopwatch.ElapsedTicks;
                Console.WriteLine("JsonGo: \t\t " + stopwatch.Elapsed);

                if (JsonGoRes > JsonNetRes)
                {
                    decimal tt = JsonGoRes / JsonNetRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X SLOWER than Json.net");
                }
                else
                {
                    decimal tt = JsonNetRes / JsonGoRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X FASTER than Json.net");
                }

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"***  DESERIALIZE {CYCLES} items***");
                //JSONNET deserialize
                stopwatch.Reset();
                stopwatch.Start();

                for (int i = 0; i < CYCLES; i++)
                {
                    var value = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<Product>>>(newtonJson);
                }

                stopwatch.Stop();
                JsonNetRes = stopwatch.ElapsedTicks;

                Console.WriteLine("Newtonsoft.Json: \t" + stopwatch.Elapsed);

                stopwatch.Reset();

                //JSONGO deserialize
                Deserializer deserializer = new Deserializer();

                stopwatch.Start();
                for (int i = 0; i < CYCLES; i++)
                {
                    var js = deserializer.Deserialize<List<List<Product>>>(jsonGoJson);
                }
                stopwatch.Stop();
                JsonGoRes = stopwatch.ElapsedTicks;
                Console.WriteLine("JsonGo: \t\t" + stopwatch.Elapsed);

                if (JsonGoRes > JsonNetRes)
                {
                    decimal tt = JsonGoRes / JsonNetRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X SLOWER than Json.net");
                }
                else
                {
                    decimal tt = JsonNetRes / JsonGoRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X FASTER than Json.net");
                }
            }
            catch (Exception ex)
            {

            }
            //Product product = new Product()
            //{
            //    Name = "ali yousefi",
            //    Age = 16,
            //    BirthDay = DateTime.Now
            //};

            ////Console.WriteLine("start");

            ////Thread.Sleep(1000);
            ////GC.Collect();
            ////GC.WaitForPendingFinalizers();
            ////GC.Collect();
            ////Console.WriteLine("ok");


            //RunJsonGo(product);
            //RunNewtonJson(product);
            //RunNewtonJson(product);
            //RunJsonGo(product);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Press ENTER to retry, or any other key to exit");
            var info = Console.ReadKey();
            if (info.Key == ConsoleKey.Enter)
            {
                var fileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                System.Diagnostics.Process.Start(fileName);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private static string Trim(string text, string textTrim)
        {
            while (text.StartsWith(textTrim))
            {
                text = text.Substring(textTrim.Length);
            }
            while (text.EndsWith(textTrim))
            {
                text = text.Substring(0, text.Length - textTrim.Length);
            }
            return text;
        }

        private static void RunNewtonJson(object data)
        {
            List<TimeSpan> all = new List<TimeSpan>();
            for (int i = 0; i < 100000; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string serialize = JsonConvert.SerializeObject(data);
                stopwatch.Stop();
                all.Add(stopwatch.Elapsed);
            }
            Console.WriteLine("newton min:" + all.Min() + " ticks: " + all.Min().Ticks);
            Console.WriteLine("newton max:" + all.Max() + " ticks: " + all.Max().Ticks);
            Console.WriteLine("newton avg:" + all.Average(x => x.Ticks));
        }


        private static void RunJsonGo(object data)
        {
            List<TimeSpan> all = new List<TimeSpan>();
            for (int i = 0; i < 100000; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string serialize2 = JsonGo.Serialize(data);
                stopwatch.Stop();
                all.Add(stopwatch.Elapsed);
                //Console.WriteLine("json go:" + stopwatch.Elapsed.ToString());
            }
            Console.WriteLine("jsonGo min:" + all.Min() + " ticks: " + all.Min().Ticks);
            Console.WriteLine("jsonGo max:" + all.Max() + " ticks: " + all.Max().Ticks);
            Console.WriteLine("jsonGo avg:" + all.Average(x => x.Ticks));

        }


        private static void RunNewtonJsonDeserialize(string json, Type type)
        {
            List<TimeSpan> all = new List<TimeSpan>();
            for (int i = 0; i < 100000; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                JsonConvert.DeserializeObject(json, type);
                stopwatch.Stop();
                all.Add(stopwatch.Elapsed);
            }
            Console.WriteLine("newton min:" + all.Min() + " ticks: " + all.Min().Ticks);
            Console.WriteLine("newton max:" + all.Max() + " ticks: " + all.Max().Ticks);
            Console.WriteLine("newton avg:" + all.Average(x => x.Ticks));
        }
        private static void RunJsonGoDeserialize(string json, Type type)
        {
            List<TimeSpan> all = new List<TimeSpan>();
            for (int i = 0; i < 100000; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Deserializer.SingleIntance.Deserialize(json, type);
                stopwatch.Stop();
                all.Add(stopwatch.Elapsed);
                //Console.WriteLine("json go:" + stopwatch.Elapsed.ToString());
            }
            Console.WriteLine("jsonGo min:" + all.Min() + " ticks: " + all.Min().Ticks);
            Console.WriteLine("jsonGo max:" + all.Max() + " ticks: " + all.Max().Ticks);
            Console.WriteLine("jsonGo avg:" + all.Average(x => x.Ticks));

        }
    }

    public class JsonGo
    {
        public static readonly Dictionary<object, WeakReference> References = new Dictionary<object, WeakReference>();
        public static string Serialize(object data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            System.Reflection.PropertyInfo[] properties = data.GetType().GetProperties();
            WeakObject chache = null;
            if (!References.TryGetValue(data, out WeakReference weakReference))
            {
                chache = new WeakObject();
                JsonGo.References.Add(data, new WeakReference(chache));
            }
            else
            {
                if (weakReference.Target is WeakObject weakObject)
                {
                    return weakObject.Data;
                }
                else
                {
                    chache = new WeakObject();
                    JsonGo.References[data] = new WeakReference(chache);
                }
            }

            stringBuilder.AppendLine("{");
            for (int i = 0; i < properties.Length; i++)
            {
                System.Reflection.PropertyInfo property = properties[i];
                stringBuilder.Append("\t");
                stringBuilder.Append("\"");
                stringBuilder.Append(property.Name);
                stringBuilder.Append("\"");
                stringBuilder.Append(":");
                stringBuilder.Append("\"");
                stringBuilder.Append(property.GetValue(data).ToString());
                stringBuilder.Append("\"");
                stringBuilder.AppendLine(",");
            }

            stringBuilder.AppendLine("}");
            chache.Data = stringBuilder.ToString();
            return chache.Data;
        }
    }

    public class WeakObject : IDisposable
    {
        public string Data { get; set; }

        ~WeakObject()
        {
            JsonGo.References.Remove(Data);
            Console.WriteLine("Destructor");
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }

    public class FastStringBuilder
    {
        private string result = "";
        public void Append(string data)
        {
            result = String.Join(string.Empty, result, data);
        }

        public void AppendLine(string data)
        {
            result = String.Join(string.Empty, result, data);
        }

        public override string ToString()
        {
            return result;
        }
    }
}
