using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Drawing;
using IronOcr;
using IronSoftware.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tesseract;
using MySql.Data.MySqlClient;
using System.Data;
using MySqlX.XDevAPI.Relational;
using System.Xml.Linq;

namespace PokemonTCGClone.Library.Models
{
    public class Card
    {
        public string CName { get; set; }
        public int HP { get; set; }
        public string cardType { get; set; }
        public string Box1Name { get; set; }
        public string Box1Text { get; set; }
        public string Box2Text { get; set; }
        public string Box2Name { get; set; }
        public string Box3Shit { get; set; }
        public string filePath { get; set; }
        public bool hasAbilityBox1 { get; set; }

        public bool ifinTable(string s)
        {
            
                string connstring;
                connstring = "server=localhost;uid=root;" +
                            "pwd=3872810Gabe$$;database=cardsdb";
                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = connstring;
                con.Open();
                string sql = "select pc_name from cardsdb.allcards where pc_name = \"" + s + "\";";

            using (var cmd = new MySqlCommand(sql, con))
            {

                if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
                    return true;
                else
                    return false;
            }
            
        }

        public void AddToMySql()
        {
            try
            {
                string connstring;
                connstring = "server=localhost;uid=root;" +
                            "pwd=3872810Gabe$$;database=cardsdb";
                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = connstring;
                con.Open();
                string sql = "INSERT INTO cardsdb.allcards(pc_name,pc_set,pc_hp,pc_hasability,pc_Box1Name,pc_Box1Text,pc_Box2Name,pc_Box2Text,pc_cardType) VALUES(@pc_name,@pc_set,@pc_hp,@pc_hasability,@pc_Box1Name,@pc_Box1Text,@pc_Box2Name,@pc_Box2Text,@pc_cardType)";

                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@pc_name", CName);
                    cmd.Parameters.AddWithValue("@pc_set", "Paldea");
                    cmd.Parameters.AddWithValue("@pc_hp", HP);
                    cmd.Parameters.AddWithValue("@pc_hasability", hasAbilityBox1);
                    cmd.Parameters.AddWithValue("@pc_Box1Name", Box1Name);
                    cmd.Parameters.AddWithValue("@pc_Box1Text", Box1Text);
                    cmd.Parameters.AddWithValue("@pc_Box2Name", Box2Name);
                    cmd.Parameters.AddWithValue("@pc_Box2Text", Box2Text);
                    cmd.Parameters.AddWithValue("@pc_cardType", cardType);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public Card()
        {

            filePath = "C:\\Users\\manng\\Desktop\\TCGClone\\PokemonTCGClone\\Resources\\Images\\card5.png";
            var ocr = new IronTesseract();
            //Name
            using (var ocrInput = new OcrInput())
            {
                var ContentArea = new CropRectangle(x: 135, y: 39, width: 336, height: 45);
                ocrInput.AddImage(filePath, ContentArea);
                var ocrResult = ocr.Read(ocrInput);
                CName = ocrResult.Text;
            }
            //Card Type
            using (var ocrInput = new OcrInput())
            {
                var ContentArea = new CropRectangle(x: 26, y: 34, width: 88, height: 24);
                ocrInput.AddImage(filePath, ContentArea);
                var ocrResult = ocr.Read(ocrInput);
                cardType = ocrResult.Text;
            }
            //HP
            using (var ocrInput = new OcrInput())
            {
                var ContentArea = new CropRectangle(x: 546, y: 33, width: 120, height: 68);
                ocrInput.AddImage(filePath, ContentArea);
                var ocrResult = ocr.Read(ocrInput);
                string temp = string.Empty;
                foreach (char c in ocrResult.Text)
                {
                    if (Char.IsDigit(c))
                    {
                        temp += c;
                    }
                }
                if (int.TryParse(temp, out int hp))
                {
                    HP = hp;
                }
            }

            //Ability Check
            using (var ocrInput = new OcrInput())
            {
                var ContentArea = new CropRectangle(x: 77, y: 556, width: 129, height: 25);
                ocrInput.AddImage(filePath, ContentArea);
                var ocrResult = ocr.Read(ocrInput);
                if (ocrResult.Text == "Ability")
                    hasAbilityBox1 = true;
                else
                    hasAbilityBox1 = false;
            }

            //Box1 Text
            using (var ocrInput = new OcrInput())
            {
                CropRectangle ContentArea;
                if (hasAbilityBox1)
                {
                    ContentArea = new CropRectangle(x: 232, y: 546, width: 364, height: 53);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    Box1Name = ocrResult.Text;
                    var ContentArea2 = new CropRectangle(x: 57, y: 593, width: 626, height: 122);
                    OcrInput ocrInput2 = new OcrInput();
                    ocrInput2.AddImage(filePath, ContentArea2);

                    var ocrResult2 = ocr.Read(ocrInput2);
                    string tempBox = string.Empty;
                    int count = 0;
                    foreach (char c in ocrResult2.Text)
                    {
                        count++;
                        if (c == '\r')
                        {
                            if (count >= ocrResult2.Text.Length - 10)
                                break;
                            else
                                continue;
                        }
                        else
                            tempBox += c;

                    }
                    Box1Text = tempBox;
                }
                else
                {
                    ContentArea = new CropRectangle(x: 44, y: 513, width: 657, height: 205);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    string tempName = string.Empty;
                    string tempBox = string.Empty;
                    bool nameStart = false;
                    bool boxStart = false;
                    if (ocrResult.Text != null)
                    {
                        foreach (char c in ocrResult.Text)
                        {

                            if (Char.IsUpper(c) && !boxStart)
                                nameStart = true;
                            if (nameStart)
                            {
                                if (Char.IsDigit(c))
                                    continue;
                                if (c != '\r' || c != '\n')
                                    tempName += c;
                                if (c == '\r' || c == '\n')
                                {
                                    nameStart = false;
                                    boxStart = true;
                                }
                            }
                            if (boxStart)
                            {
                                tempBox += c;
                            }
                        }
                        Box1Name = tempName;
                        Box1Text = tempBox;
                    }
                }
            }

            //Box2 Name
            using (var ocrInput = new OcrInput())
            {
                CropRectangle ContentArea;
                if (hasAbilityBox1)
                {
                    ContentArea = new CropRectangle(x: 190, y: 705, width: 352, height: 80);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    Box2Name = ocrResult.Text;
                }
                else
                {
                    ContentArea = new CropRectangle(x: 177, y: 735, width: 329, height: 45);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    Box2Name = ocrResult.Text;
                }
            }


            //Box2 Text
            using (var ocrInput = new OcrInput())
            {
                CropRectangle ContentArea;
                if (hasAbilityBox1)
                {
                    ContentArea = new CropRectangle(x: 62, y: 775, width: 612, height: 104);
                    ocrInput.AddImage(filePath, ContentArea);
                    ocrInput.ReplaceColor(System.Drawing.Color.Black, System.Drawing.Color.Black, -5);
                    var ocrResult = ocr.Read(ocrInput);
                    Box2Text = ocrResult.Text;
                }
                else
                {
                    ContentArea = new CropRectangle(x: 43, y: 729, width: 654, height: 120);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    Box2Text = ocrResult.Text;
                }
            }


        }
        public Card(string s)
        {
            
            filePath = s;
            var ocr = new IronTesseract();
            //Name
            using (var ocrInput = new OcrInput())
            {
                var ContentArea = new CropRectangle(x: 135, y: 39, width: 336, height: 45);
                ocrInput.AddImage(filePath,ContentArea);
                var ocrResult = ocr.Read(ocrInput);
                CName = ocrResult.Text;
            }


            if (!ifinTable(CName))
            {

                //Card Type
                using (var ocrInput = new OcrInput())
                {
                    var ContentArea = new CropRectangle(x: 26, y: 34, width: 88, height: 24);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    cardType = ocrResult.Text;
                }
                //HP
                using (var ocrInput = new OcrInput())
                {
                    var ContentArea = new CropRectangle(x: 546, y: 33, width: 120, height: 68);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    string temp = string.Empty;
                    foreach (char c in ocrResult.Text)
                    {
                        if (Char.IsDigit(c))
                        {
                            temp += c;
                        }
                    }
                    if (int.TryParse(temp, out int hp))
                    {
                        HP = hp;
                    }
                }

                //Ability Check
                using (var ocrInput = new OcrInput())
                {
                    var ContentArea = new CropRectangle(x: 77, y: 556, width: 129, height: 25);
                    ocrInput.AddImage(filePath, ContentArea);
                    var ocrResult = ocr.Read(ocrInput);
                    if (ocrResult.Text == "Ability")
                        hasAbilityBox1 = true;
                    else
                        hasAbilityBox1 = false;
                }

                //Box1 Text
                using (var ocrInput = new OcrInput())
                {
                    CropRectangle ContentArea;
                    if (hasAbilityBox1)
                    {
                        ContentArea = new CropRectangle(x: 232, y: 546, width: 364, height: 53);
                        ocrInput.AddImage(filePath, ContentArea);
                        var ocrResult = ocr.Read(ocrInput);
                        Box1Name = ocrResult.Text;
                        var ContentArea2 = new CropRectangle(x: 57, y: 593, width: 626, height: 122);
                        OcrInput ocrInput2 = new OcrInput();
                        ocrInput2.AddImage(filePath, ContentArea2);

                        var ocrResult2 = ocr.Read(ocrInput2);
                        string tempBox = string.Empty;
                        int count = 0;
                        foreach (char c in ocrResult2.Text)
                        {
                            count++;
                            if (c == '\r')
                            {
                                if (count >= ocrResult2.Text.Length - 10)
                                    break;
                                else
                                    continue;
                            }
                            else
                                tempBox += c;

                        }
                        Box1Text = tempBox;
                    }
                    else
                    {
                        ContentArea = new CropRectangle(x: 44, y: 513, width: 657, height: 205);
                        ocrInput.AddImage(filePath, ContentArea);
                        var ocrResult = ocr.Read(ocrInput);
                        string tempName = string.Empty;
                        string tempBox = string.Empty;
                        bool nameStart = false;
                        bool boxStart = false;
                        if (ocrResult.Text != null)
                        {
                            foreach (char c in ocrResult.Text)
                            {

                                if (Char.IsUpper(c) && !boxStart)
                                    nameStart = true;
                                if (nameStart)
                                {
                                    if (Char.IsDigit(c))
                                        continue;
                                    if (c != '\r' || c != '\n')
                                        tempName += c;
                                    if (c == '\r' || c == '\n')
                                    {
                                        nameStart = false;
                                        boxStart = true;
                                    }
                                }
                                if (boxStart)
                                {
                                    tempBox += c;
                                }
                            }
                            Box1Name = tempName;
                            Box1Text = tempBox;
                        }
                    }
                }

                //Box2 Name
                using (var ocrInput = new OcrInput())
                {
                    CropRectangle ContentArea;
                    if (hasAbilityBox1)
                    {
                        ContentArea = new CropRectangle(x: 190, y: 705, width: 352, height: 80);
                        ocrInput.AddImage(filePath, ContentArea);
                        var ocrResult = ocr.Read(ocrInput);
                        Box2Name = ocrResult.Text;
                    }
                    else
                    {
                        ContentArea = new CropRectangle(x: 177, y: 735, width: 329, height: 45);
                        ocrInput.AddImage(filePath, ContentArea);
                        var ocrResult = ocr.Read(ocrInput);
                        Box2Name = ocrResult.Text;
                    }
                }


                //Box2 Text
                using (var ocrInput = new OcrInput())
                {
                    CropRectangle ContentArea;
                    if (hasAbilityBox1)
                    {
                        ContentArea = new CropRectangle(x: 62, y: 775, width: 612, height: 104);
                        ocrInput.AddImage(filePath, ContentArea);
                        ocrInput.ReplaceColor(System.Drawing.Color.Black, System.Drawing.Color.Black, -5);
                        var ocrResult = ocr.Read(ocrInput);
                        Box2Text = ocrResult.Text;
                    }
                    else
                    {
                        ContentArea = new CropRectangle(x: 43, y: 729, width: 654, height: 120);
                        ocrInput.AddImage(filePath, ContentArea);
                        var ocrResult = ocr.Read(ocrInput);
                        Box2Text = ocrResult.Text;
                    }
                }
                AddToMySql();
            }
            
           
        }
            
            //End of Card

        public void setFilepath(string s)
        {
            filePath = s;
        }
        
    }
}
