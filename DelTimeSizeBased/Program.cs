using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelTimeSizeBased
{
    class Program
    {
        static void Main(string[] args)
        {
            // Garbage collection
            bool Dryrun = true;
            foreach(string s in args)
            {
                if (s == "DEL")
                {
                    Dryrun = false;
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //getfile list
            if (Directory.Exists(Settings.Default.dir))
            {
                var dirinfo = new DirectoryInfo(Settings.Default.dir);
                if (Dryrun)
                {
                    Console.WriteLine(" DRY RUN");
                }
                else
                {
                    Console.WriteLine(" WORKING RUN");
                }
                int countdown = Settings.Default.BigCount;
                string status = "[ OK  ]";
                foreach (FileInfo f in dirinfo.GetFiles().OrderByDescending(t => t.LastWriteTime))
                {
                    if (countdown <= 0)
                    {
                        status = "[ DEL ]";
                        if (!Dryrun)
                        {
                            // reaalselt kustutame
                            try
                            {
                                f.Delete();
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine("Ei õnnestunud kustutada: " + f.Name);
                            }

                            
                        }
                    }
                    if (f.Length > Settings.Default.BigBytes) { countdown--; }
                    

                    Console.WriteLine(status+f.LastWriteTime.ToString() + " " + f.Length.ToString()+ " " + f.Name);
                }

            }
            else { Console.WriteLine(string.Format(" Cant find Directory \"{0}\" ", Settings.Default.dir)); }


            // Console.ReadKey();
        }
    }
}
