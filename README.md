# ๐ C# ์ธ์ด๋ฒ์ญ ํ๋ก๊ทธ๋จ
- <b>Language</b> : <img alt="C#" src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white"/>
- <b>Tool</b> : <img alt="Visual Studio" src="https://img.shields.io/badge/Visual Studio-5C2D91?style=flat-square&logo=Visual Studio&logoColor=white"/>
- ๋ค์ด๋ฒ์ ํํ๊ณ  API๋ฅผ ํ์ฉํ์์ต๋๋ค.
- ์ด๋ฏธ์ง ์ธ์์ ์ํด ์คํ ์์ค OCR์ธ ํ์๋ํธ OCR์์ง์ ์ฌ์ฉํ์์ต๋๋ค.

<br>

## ๐ ์ ์๊ธฐ๊ฐ ๋ฐ ๊ฐ๋ฐ ์ธ์
- ๊ธฐ๊ฐ : 2022.01 ~ 2022.01 (์ฝ 1์ฃผ)
- ์ธ์ : 3๋ช
- ๋ด๋น ์ญํ  : Winform์ ํ์ฉํ UI๊ตฌํ

<br>

## ๐ ์ฃผ์ ๊ธฐ๋ฅ

#### ๐ธ ์ธ์ด๋ฒ์ญ
- ๋ฒ์ญ๊ธฐ API๋ฅผ ์ด์ฉํ์ฌ ์๋ ฅ๋ ํ์คํธ๋ฅผ ์ ์กํฉ๋๋ค.
- ๋ฒ์ญ๋ ํ์คํธ๋ฅผ JSONํ์์ผ๋ก ๋ฐ์์ ํ๋ก๊ทธ๋จ์ ์ถ๋ ฅ๋ฉ๋๋ค.

<img src="img/main.png" width="540" height="330" >

```c#
HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

//Header์ ์ ๋ณด ์ถ๊ฐ
request.Headers.Add("X-Naver-Client-Id", " ");
request.Headers.Add("X-Naver-Client-Secret", " ");
request.Method = "POST";
tring query = textBox1.Text; //๋ฒ์ญํ๊ณ ์ ํ๋ ๋ฌธ์ฅ.

//source ko, target en ํ->์ query๋ฌธ์ฅ์ ๋ฒ์ญ.
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

//Json parsing
JObject ret = JObject.Parse(text);
textBox2.Text = ret["message"]["result"]["translatedText"].ToString(); //message
```

- ๋ฒ์ญ ๋๋ ์ธ์ด๋ฅผ ์ ํํ์ง์์๋ ์๋์ผ๋ก ๊ฐ์งํฉ๋๋ค.
<img src="img/auto.png" width="540" height="330" >

```c#
string url2 = "https://openapi.naver.com/v1/papago/detectLangs";
HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url2);
//Header์ ์ ๋ณด ์ถ๊ฐ
request2.Headers.Add("X-Naver-Client-Id", " ");
request2.Headers.Add("X-Naver-Client-Secret", " ");
request2.Method = "POST";
string query2 = textBox1.Text; //๊ฐ์งํ๊ณ ์ ํ๋ ๋ฌธ์ฅ.

//query๋ฌธ์ฅ์ ๊ฐ์ง
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
```

#### ๐ธ ์ด๋ฏธ์ง ๋ฒ์ญ
- OCR์์ง์ด ์ฝ๊ฒ ์ธ์ํ๊ฒํ๊ธฐ ์ํด ์ด๋ฏธ์ง์ ํฝ์์ ์ด์งํํ์ฌ ๊ฒ์์๊ณผ ํฐ์์ผ๋ก๋ง ํํ๋๋ ์ด๋ฏธ์ง๋ฅผ ๋ง๋ค์ด์ค๋๋ค.
- ๊ณ์ฐ๋ ๋ฐ์ดํฐ๋ฅผ result์ ๋ด์ ์ ๋ณด๋ฅผ ์ถ์ถํ์ฌ ๋ฌธ์์ด์ ํ์คํธ๋ฐ์ค์ ๋ด์์ค๋๋ค.

<img src="img/japWord.png" width="300" height="120">
<img src="img/image.png" width="540" height="330">

```c#
 Bitmap bmp = new Bitmap(imgfile);
pictureBox1.Image = bmp;



//binary ์ด๋ฏธ์ง๋ฅผ ์ฝ๊ธฐ ์ฝ๋๋ก ์ด์งํ (ํฝ์๋น ์๊ฐ๊ณผ ๋ช์๊ฐ์ ์์ค๋ค)
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
      var engine = new TesseractEngine(@"./tessdata", language, EngineMode.TesseractAndLstm);  //tessdata(๋ฌธ์์ธ์ ์ธํ๊ฐ) ์์น
      var result = engine.Process(pix);

      textBox1.Text = result.GetText().TrimStart();  //๊ณต๋ฐฑ ์ ๊ฑฐํ์ฌ ํ์คํธ๋ฐ์ค์ ์ฝ์

      pictureBox1.Image = pictureBox1.Image;

}
catch
{
      MessageBox.Show("์คํํ์ผ์ด ์๋ ํด๋์ tessdata ํด๋๋ฅผ ์ฝ์ํด์ฃผ์ธ์!!");
}
```

#### ๐ธ URL์ ํตํ ๋ฒ์ญ๋ด์ฉ ๊ฒ์
- URL์ ํตํด ๋ฒ์ญํ  ์ธ์ด, ๋ฒ์ญ๋  ์ธ์ด, ๋ฒ์ญํ  ํ์คํธ๋ฅผ ์ ์กํ์ฌ ํํ๊ณ ์์ ์ง์  ๋จ์ด ์ ๋ณด๋ฅผ ๊ฒ์ํด ๋ณผ ์ ์๋๋ก ๊ตฌํํ์์ต๋๋ค.

<img src="img/papagoLink.png" width="540" height="260">

```c#
string source = "ko", target = "en";

switch (comboBox1.Text)
{
  case "ํ๊ธ":
    source = "ko";
    break;
  case "์์ด":
    source = "en";
    break;
  case "์ผ๋ณธ์ด":
    source = "ja";
    break;
  default:
    source = Aoto();
    break;
}
switch (comboBox2.Text)
{
  case "ํ๊ธ":
    target = "ko";
    break;
  case "์์ด":
    target = "en";
    break;
  case "์ผ๋ณธ์ด":
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
```

#### ๐ธ ๋์์ธ 
- ํฐํธ ํต์ผ, ๋ฒํผ ๋์์ธ, ์ค๋ธ์ ํธ ์ฌ๋ฐฐ์น์ ์์ด์ฝ ์ฝ์ ๋ฑ 
- ํ๋ฉด์ ์๋ก ๊ตฌ์ฑ ์ ์ํ ํผ ๋์์ธ์ผ๋กย ์ฌ์ฉ์์ ํธ์์ ๊ฐ์์ฑ์ ํ๋ ์ํค๋ ๋ฐฉํฅ์ผ๋ก UI๋ฅผ ๋์์ธ ํ์์ต๋๋ค.

<img src="img/design.png" width="800" height="300">

# ๐ ๊ฐ์ฌํฉ๋๋ค.
