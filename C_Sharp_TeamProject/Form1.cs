using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Tesseract;

namespace Project
{
    public partial class Form1 : Form
    {

        string target = "en";
        public Form1()
        {
            InitializeComponent();
        }

        public string Aoto()
        {
            string url2 = "https://openapi.naver.com/v1/papago/detectLangs";
            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url2);
            //Header에 정보 추가
            request2.Headers.Add("X-Naver-Client-Id", "mAJtZQ50ak9cUh_VAuee");
            request2.Headers.Add("X-Naver-Client-Secret", "jonD5WI6RG");
            request2.Method = "POST";
            string query2 = textBox1.Text; //감지하고자 하는 문장.

            //query문장을 감지
            byte[] byteDataParams2 = Encoding.UTF8.GetBytes("query=" + query2);

            request2.ContentType = "application/x-www-form-urlencoded";
            request2.ContentLength = byteDataParams2.Length;
            //request
            Stream rqstream2 = request2.GetRequestStream();
            rqstream2.Write(byteDataParams2, 0, byteDataParams2.Length);
            rqstream2.Close();

            //response
            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
            Stream rpstream2 = response2.GetResponseStream();
            StreamReader reader2 = new StreamReader(rpstream2, Encoding.UTF8);

            string text2 = reader2.ReadToEnd();


            //Json parsing
            JObject ret2 = JObject.Parse(text2);
            string source = ret2["langCode"].ToString().Trim(); //message
            return source;
        }
        private void button1_Click_1(object sender, EventArgs e)    // 번역기 버튼
        {
            string source="ko";
            //요청 url 설정
            textBox1.Text = textBox1.Text.TrimStart();  //공백 제거하여 텍스트박스에 삽입 (오류 방지)

            if (textBox1.Text == "")
            {
                MessageBox.Show("번역할 문장을 입력하세요!");
                return;
            }
            
            switch (comboBox1.Text)
            {
                case "한글":
                    source = "ko";
                    break;
                case "영어":
                    source = "en";
                    break;
                case "일본어":
                    source = "ja";
                    break;
                default:
                    source = Aoto();
                    break;

            }
            switch (comboBox2.Text)
            {
                case "한글":
                    target = "ko";
                    break;
                case "영어":
                    target = "en";
                    break;
                case "일본어":
                    target = "ja";
                    break;
            }
            if(source== target)
            {
                textBox2.Text = textBox1.Text;
                MessageBox.Show("번역할 언어가 번역언어와 같아요!");
                return;
            }



            string url = "https://openapi.naver.com/v1/papago/n2mt";
            

            //url을 사용해서 httprequest생성
            




            

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);



                //Header에 정보 추가
                request.Headers.Add("X-Naver-Client-Id", "mAJtZQ50ak9cUh_VAuee");
                request.Headers.Add("X-Naver-Client-Secret", "jonD5WI6RG");
                request.Method = "POST";
                string query = textBox1.Text; //번역하고자 하는 문장.

                //source ko, target en 한->영 query문장을 번역.
                byte[] byteDataParams = Encoding.UTF8.GetBytes("source=" + source + "&target=" + target + "&text=" + query);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteDataParams.Length;
                //request
                Stream rqstream = request.GetRequestStream();
                rqstream.Write(byteDataParams, 0, byteDataParams.Length);
                rqstream.Close();

                //response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream rpstream = response.GetResponseStream();
                StreamReader reader = new StreamReader(rpstream, Encoding.UTF8);

                string text = reader.ReadToEnd();


                //textBox2.Text = text;

                //Json parsing
                JObject ret = JObject.Parse(text);
                textBox2.Text = ret["message"]["result"]["translatedText"].ToString(); //message
            
   


        }


        private void button2_Click(object sender, EventArgs e)  //언어 교환 버튼
        {
            string swap;
            
            if (comboBox1.Text == "언어감지")
            {
                target = Aoto();
                comboBox1.Text = "감지("+ target + ")";
                
            }
            swap = comboBox1.Text;
            comboBox1.Text = comboBox2.Text;
            comboBox2.Text = swap;

            textBox1.Text = textBox2.Text;
            textBox2.Text = "";
            

        }

        private void button3_Click(object sender, EventArgs e)  //이미지 인식 버튼
        {



            string language = "kor";

            switch (comboBox3.Text)
            {
                case "한글":
                    language = "kor";
                    break;
                case "영어":
                    language = "eng";
                    break;
                case "일본어":
                    language = "jpn";
                    break;

                default:
                    MessageBox.Show("단어 이미지 인식을 할 언어를 선택 해주세요!");
                    return;
            }


            
            string imgfile = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgfile = dialog.FileName;
            }

            try             //이미지파일 불러오는것을 취소할때 오류방지
            {
                Bitmap bmp = new Bitmap(imgfile);
                pictureBox1.Image = bmp;



                //binary 이미지를 읽기 쉽도록 이진화 (픽셀당 색값과 명암값을 없앤다)
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        Color c = bmp.GetPixel(i, j);
                        int binary = (c.R + c.G + c.B) / 3;

                        if (binary > 200)
                            bmp.SetPixel(i, j, Color.Black);
                        else
                            bmp.SetPixel(i, j, Color.White);
                    }
                }

                Pix pix = PixConverter.ToPix(bmp);
                try
                {
                    var engine = new TesseractEngine(@"./tessdata", language, EngineMode.TesseractAndLstm);  //tessdata(문자인식 세팅값) 위치
                    var result = engine.Process(pix);

                    textBox1.Text = result.GetText().TrimStart();  //공백 제거하여 텍스트박스에 삽입


                    pictureBox1.Image = pictureBox1.Image;

                }
                catch
                {
                    MessageBox.Show("실행파일이 있는 폴더에 tessdata 폴더를 삽입해주세요!!");
                }



            }
            catch { }



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = comboBox1.Text;
            label4.Text = comboBox1.Text+" 번역기";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Text = comboBox2.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string source = "ko", target = "en";

            switch (comboBox1.Text)
            {
                case "한글":
                    source = "ko";
                    break;
                case "영어":
                    source = "en";
                    break;
                case "일본어":
                    source = "ja";
                    break;
                default:
                    source = Aoto();
                    break;
            }
            switch (comboBox2.Text)
            {
                case "한글":
                    target = "ko";
                    break;
                case "영어":
                    target = "en";
                    break;
                case "일본어":
                    target = "ja";
                    break;
            }

            if (textBox1.Text == "")
            {
                System.Diagnostics.Process.Start("https://papago.naver.com/");

            }
            else
            {
                System.Diagnostics.Process.Start($"https://papago.naver.com/?sk={source}&tk={target}&st={textBox1.Text}");
            }



        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
