using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;



namespace Logic_Designer
{

    public partial class NumberedRichTextBox : UserControl
    {
        public NumberedRichTextBox()
        {
            InitializeComponent();
            textComboBox1.Items.Add("txt");
            textComboBox1.SelectedItem = "txt";
            initSyntaxGroup();
            initRegex();
        }

        private const string syntaxFileName = "syntax.xml";

        public static List<syntaxLang> langSyntax = new List<syntaxLang>();

        [DllImport("user32.dll")] // import lockwindow to remove flashing
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        //urcuje ci sa ma vykonavat pri text update highilght syntaxe po riadkoch (true znamena ze je zakazane zvyraznovanie)
        static bool highLightLock = false;

        //koncovka otvoreneho suboru / resp. suboru na ulozenie (podla tohohto sa urci ako sa ma highlightovat)
        public static string extension = "txt";
        public static string language = "txt";

        //nacitanie syntaxe do skupin
        public static Regex riadkovyKomentar;
        public static Regex komentarStart;
        public static Regex komentarKoniec;
        public static Regex skupina1;
        public static Regex skupina2;
        public static Regex skupina3;
        public static Regex skupina4;
        public static Regex skupina5;
        public static Regex skupina6;
        public static Regex cisla;
        //public Regex znaky = new Regex(@"[!;()=><:,./+\-[\]|?{}*^&%~]", RegexOptions.IgnoreCase);

        //urcuje ktore skupiny sa maju highlightovat
        private static bool hlRiadkovyKomentar = false;
        //private static bool hlKomentarStart = false;
        //private static bool hlKomentarKoniec = true; 
        private static bool hlSkupina1 = false;
        private static bool hlSkupina2 = false;
        private static bool hlSkupina3 = false;
        private static bool hlSkupina4 = false;
        private static bool hlSkupina5 = false;
        private static bool hlSkupina6 = false;
        private static bool hlCisla = false;
        //private static bool hlZnaky = false;

        //spusti najprv zvolenie jazyka na zaklade pripony a nsledne sa vykona inicializacia regexov (volane z chooseLanguage)
        public void setExtension(string ext)
        {
            if (ext != extension)
            {
                extension = ext;
                //vzdy ked sa zmeni jazyk highlitovania tak nastavim jazyk jazyk
                chooseLanguage();
            }
        }

        //inicializovanie regexov podla prislusneho jazyka 
        public static void initRegex()
        {
            //(?<=^|\s)5 p\.m\.(?=\s|$)
            riadkovyKomentar = new Regex(@getSyntaxKeywords("riadkovyKomentar"));
            //komentarStart = new Regex(@getSyntaxKeywords("komentarStart"));
            //komentarKoniec = new Regex(@getSyntaxKeywords("komentarKoniec"));


            skupina1 = new Regex(@"(?<=^|[.'=();:\s])(" + getSyntaxKeywords("skupina1") + @")(?=$|[.'=();:\s])", RegexOptions.IgnoreCase);
            skupina2 = new Regex(@"(?<=^|[.'=();:\s])(" + getSyntaxKeywords("skupina2") + @")(?=$|[.'=();:\s])", RegexOptions.IgnoreCase);
            skupina3 = new Regex(@"(?<=^|[.'=();:\s])(" + getSyntaxKeywords("skupina3") + @")(?=$|[.'=();:\s])", RegexOptions.IgnoreCase);
            skupina4 = new Regex(@"(?<=^|[.'=();:\s])(" + getSyntaxKeywords("skupina4") + @")(?=$|[.'=();:\s])", RegexOptions.IgnoreCase);
            skupina5 = new Regex(@"(?<=^|[.'=();:\s])(" + getSyntaxKeywords("skupina5") + @")(?=$|[.'=();:\s])", RegexOptions.IgnoreCase);
            skupina6 = new Regex(@"(?<=^|[.'=();:\s])(" + getSyntaxKeywords("skupina6") + @")(?=$|[.'=();:\s])", RegexOptions.IgnoreCase);

            /*
            skupina1 = new Regex(@"\b(" + getSyntaxKeywords("skupina1") + ")" + @"\b", RegexOptions.IgnoreCase);
            skupina2 = new Regex(@"\b(" + getSyntaxKeywords("skupina2") + ")" + @"\b", RegexOptions.IgnoreCase);
            skupina3 = new Regex(@"\b(" + getSyntaxKeywords("skupina3") + ")" + @"\b", RegexOptions.IgnoreCase);
            skupina4 = new Regex(@"\b(" + getSyntaxKeywords("skupina4") + ")" + @"\b", RegexOptions.IgnoreCase);
            skupina5 = new Regex(@"\b(" + getSyntaxKeywords("skupina5") + ")" + @"\b", RegexOptions.IgnoreCase);
            skupina6 = new Regex(@"\b(" + getSyntaxKeywords("skupina6") + ")" + @"\b", RegexOptions.IgnoreCase);
            */

            cisla = new Regex(@"\b([0-9]+)\b", RegexOptions.IgnoreCase);

            //MessageBox.Show("hlRiadkovyKomentar="+hlRiadkovyKomentar.ToString()+"\nhlSkupina1="+hlSkupina1.ToString()+"\nhlSkupina2="+hlSkupina2.ToString()+"\nhlSkupina3="+hlSkupina3.ToString()+"\nhlSkupina4="+hlSkupina4.ToString()+"\nhlSkupina5="+hlSkupina5.ToString()+"\nhlSkupina6="+hlSkupina6.ToString()+"\nhlCisla="+hlCisla.ToString());
        }

        //nastavi globalnu premennu language - urcuje ktorym jazykom sa bude zvyraznovat syntax
        public static void chooseLanguage()
        {
            int match = -1;

            //prejde vsetky jazyky
            for (int i = 0; i < langSyntax.Count; i++)
            {
                //porovna sa pripona ulozena v objekte jazyka (langSyntax[i].extension) a aktualnym suborom (extension)
                Regex tmpExtension = new Regex(@"\b(" + langSyntax[i].extension + @")\b", RegexOptions.IgnoreCase);
                //MessageBox.Show(langSyntax[i].language + " " + tmpExtension.Match(extension).Success.ToString() + " " +extension);
                if (tmpExtension.Match(extension).Success)
                {
                    match = i;
                    break;
                }
                //MessageBox.Show(i.ToString() + "\n" + langSyntax[i].language + "\n" + langSyntax.Count.ToString());
            }

            //ak som nasiel k pripone odpovedajuci jazyk, tak ulozim jazyk do globalnej premennej + spustim zvyraznovanie cisel
            if (match >= 0 && match < langSyntax.Count)
            {
                language = langSyntax[match].language;
                textComboBox1.SelectedIndex = match + 1; // magic, dont touch! Nastavi ComboBox kde sa vybera jazyk na zvyraznenie na potrebnu hodnotu
                //ked sa zmenil jazyk tak je nutne zmenit regexi (syntax) 
                initRegex();
            }
            else
            {
                //ak som jazyk nenasiel, tak sa ne-highlightuje
                language = "txt";
                initRegex();
            }
            //MessageBox.Show(language);
        }


        //nacitat syntax zo syntax.xml a inicializovat prislusne regexi
        public static void initSyntaxGroup()
        {
            try
            {
                //otvorit subor kde su jazyky
                XmlTextReader xmlContent = new XmlTextReader(syntaxFileName);

                //prejdem na prvy jazyk
                while (xmlContent.ReadToFollowing("Jazyk"))
                {
                    //vytvorim si pomocne premenne (docasny objekt typu syntaxLang -> potom ho vlozim do listu)
                    XmlReader inner = xmlContent.ReadSubtree();
                    syntaxLang tmpSyntax = new syntaxLang();
                    bool saveLanguage = true;

                    //nazov jazyka
                    xmlContent.MoveToFirstAttribute();
                    tmpSyntax.language = xmlContent.Value;

                    //skontrolujem ci uz nahodou nie je jazyk ulozeny
                    for (int i = 0; i < langSyntax.Count; i++)
                    {
                        if (langSyntax[i].language == tmpSyntax.language)
                        {
                            saveLanguage = false;
                            break;
                        }
                    }

                    //ak este nie je jazyk ulozeny tak ho pridam
                    if (saveLanguage == true)
                    {
                        //koncovka suboru
                        if (inner.ReadToFollowing("Koncovka"))
                        {
                            tmpSyntax.extension = inner.ReadElementString();
                           
                            //klucove slova
                            while (inner.ReadToFollowing("KlucoveSlova"))
                            {
                                xmlContent.MoveToFirstAttribute();
                                switch (inner.Value)
                                {

                                    case "riadkovyKomentar":
                                        tmpSyntax.riadkovyKomentar = inner.ReadElementString();
                                        break;

                                    case "komentarStart":
                                        tmpSyntax.komentarStart = inner.ReadElementString();
                                        break;

                                    case "komentarKoniec":
                                        tmpSyntax.komentarKoniec = inner.ReadElementString();
                                        break;

                                    case "skupina1":
                                        
                                    case "skupina2":
                                        
                                    case "skupina3":
                                       
                                    case "skupina4":
                                       
                                    case "skupina5":
                                        
                                    case "skupina6":
                                        string tmpString = inner.ReadElementString();
                                        //ak sa nachadza v klucovych slovach $ tak ho treba nahradit '\$' aby sa v regexe neskor escapol
                                        tmpString = tmpString.Replace("$", "\\$");
                                        tmpSyntax.keywords.Add(tmpString);
                                        break;

                                    default:
                                        break;
                                }
                            }

                            //vlozim nakoniec do globalneho listu vsetkych nacitanych jazykov
                            langSyntax.Add(tmpSyntax);

                            /*
                            //pomocny vypis nacitaneho xml suboru
                            string tmpStr2 = "";
                            for (int i = 0; i < tmpSyntax.keywords.Count; i++)
                            {
                                tmpStr2 = tmpStr2 + "\n\n" + tmpSyntax.keywords[i];
                            }
                            MessageBox.Show(tmpSyntax.language + "\n\n" + tmpSyntax.extension + "\n\n" + tmpSyntax.riadkovyKomentar + "\n\n" + tmpSyntax.komentarStart + "\n\n" + tmpSyntax.komentarKoniec + "\n\n" + tmpStr2);
                            */

                            //nakoniec vlozim jazyk do listboxu1
                            textComboBox1.Items.Add(tmpSyntax.language);
                        }
                    }
                }
            }
            catch (Exception es)
            {
                //return false;
                MessageBox.Show(es.Message);
            }
        }

        //ziskat obsah skupiny z objektov langSyntax na zaklade globalnej premennej extension 
        private static string getSyntaxKeywords(string keywordGroup)
        {
            /*
            string output;
            string name = "";

            //otvorit subor kde su jazyky
            XmlTextReader xmlContent = new XmlTextReader("syntax.xml");
            */

            int match = -1;
            for (int i = 0; i < langSyntax.Count; i++)
            {
                if (langSyntax[i].language == language)
                {
                    match = i;
                    break;
                }
            }


            //podla posunutej skupiny sa vrati hodnota stringu (obsahuje slova syntaxe pre danu skupinu)
            switch (keywordGroup)
            {

                case "riadkovyKomentar":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].riadkovyKomentar.Length > 0)
                    {
                        hlRiadkovyKomentar = true;
                        return langSyntax[match].riadkovyKomentar;
                    }
                    else
                    {
                        hlRiadkovyKomentar = false;
                        return "";
                    }

                /*
            case "komentarStart":
                if (match >= 0 && match < langSyntax.Count && langSyntax[match].komentarStart.Length > 0)
                {
                    //MessageBox.Show(langSyntax[match].komentarStart);
                    hlKomentarStart = true;
                    return langSyntax[match].komentarStart;
                }
                else
                {
                    hlKomentarStart = false;
                    return "";
                }

            case "komentarKoniec":
                if (match >= 0 && match < langSyntax.Count && langSyntax[match].komentarKoniec.Length > 0)
                {
                    //MessageBox.Show(langSyntax[match].komentarKoniec);
                    hlKomentarKoniec = true;
                    return langSyntax[match].komentarKoniec;
                }
                else
                {
                    hlKomentarKoniec = false;
                    return "";
                }

                 */

                case "skupina1":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].keywords.Count > 0 && langSyntax[match].keywords[0].Length > 0)
                    {
                        hlSkupina1 = true;
                        hlCisla = true;  //ak mam aspon jednu skupinu tak highlightujem aj cisla
                        //hlZnaky = true;
                        return langSyntax[match].keywords[0];
                    }
                    else
                    {
                        hlSkupina1 = false;
                        hlCisla = false;
                        //hlZnaky = false;
                        return "";
                    }

                case "skupina2":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].keywords.Count > 1 && langSyntax[match].keywords[1].Length > 0)
                    {
                        hlSkupina2 = true;
                        return langSyntax[match].keywords[1];
                    }
                    else
                    {
                        hlSkupina2 = false;
                        return "";
                    }

                case "skupina3":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].keywords.Count > 2 && langSyntax[match].keywords[2].Length > 0)
                    {
                        hlSkupina3 = true;
                        return langSyntax[match].keywords[2];
                    }
                    else
                    {
                        hlSkupina3 = false;
                        return "";
                    }

                case "skupina4":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].keywords.Count > 3 && langSyntax[match].keywords[3].Length > 0)
                    {
                        hlSkupina4 = true;
                        return langSyntax[match].keywords[3];
                    }
                    else
                    {
                        hlSkupina4 = false;
                        return "";
                    }

                case "skupina5":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].keywords.Count > 4 && langSyntax[match].keywords[4].Length > 0)
                    {
                        hlSkupina5 = true;
                        return langSyntax[match].keywords[4];
                    }
                    else
                    {
                        hlSkupina5 = false;
                        return "";
                    }

                case "skupina6":
                    if (match >= 0 && match < langSyntax.Count && langSyntax[match].keywords.Count > 5 && langSyntax[match].keywords[5].Length > 0)
                    {
                        hlSkupina6 = true;
                        return langSyntax[match].keywords[5];
                    }
                    else
                    {
                        hlSkupina6 = false;
                        return "";
                    }

                //ak sa nahodou netrafim do ani jednej skupiny, tak zakazem vsetko highlightovanie
                default:
                    hlRiadkovyKomentar = false;
                    //hlKomentarStart = false;
                    //hlKomentarKoniec = false; 
                    hlSkupina1 = false;
                    hlSkupina2 = false;
                    hlSkupina3 = false;
                    hlSkupina4 = false;
                    hlSkupina5 = false;
                    hlSkupina6 = false;
                    hlCisla = false;
                    //hlZnaky = false;
                    return "";
            }

            /*
            //dokym nenarazim na spravnu skupinu (slova syntaxe) tak prechadzam klucove slova
            do
            {
                xmlContent.ReadToFollowing("KlucoveSlova");
                xmlContent.MoveToFirstAttribute();
                name = xmlContent.Value;
            }
            while (!name.Equals(keywordGroup) && name.Length != 0);

            output = xmlContent.ReadElementString();
            //MessageBox.Show(@"\b(" + output + ")"+@"\b");
            return output;
             */
        }

        //otestuje sa riadok voci posunutemu regexp (skupina) a zvyraznia sa slova danou farbou
        private void matchRegexp(Color color, Regex skupina, int curChar, int line)
        {
            //ak existuje curLine
            if (line >= 0 && line < textRichTextBox1.Lines.Length)
            {
                int fisrtLineChar = textRichTextBox1.GetFirstCharIndexFromLine(line);

                if (skupina.Equals(riadkovyKomentar))
                {
                    //otestovat riadok proti regexp - pre riadkovyKomentar
                    foreach (Match klucoveSlovo in skupina.Matches(textRichTextBox1.Lines[line]))
                    {
                        //klucoveSlovo.Index + fisrtLineChar ->pozicia slova v riadku + znaky pred danym riadkom
                        //textRichTextBox1.Lines[line].Length - klucoveSlovo.Index ->pocet znakov od najdeneho slova po koniec riadku
                        textRichTextBox1.Select(klucoveSlovo.Index + fisrtLineChar, textRichTextBox1.Lines[line].Length - klucoveSlovo.Index);
                        textRichTextBox1.SelectionColor = color; //zafarbim najdene slovo
                    }
                    textRichTextBox1.SelectionLength = 0; //deselectnem
                    textRichTextBox1.SelectionStart = curChar; //nastavim kurzor na povodnu hodnotu
                    textRichTextBox1.SelectionColor = Color.Black; //nastavim farbu na ciernu

                }
                else
                {
                    //otestovat riadok proti regexp - postupne kazdu skupinu
                    foreach (Match klucoveSlovo in skupina.Matches(textRichTextBox1.Lines[line]))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        textRichTextBox1.Select(klucoveSlovo.Index + fisrtLineChar, klucoveSlovo.Length);
                        textRichTextBox1.SelectionColor = color; //zafarbim najdene slovo
                    }
                    textRichTextBox1.SelectionLength = 0; //deselectnem
                    textRichTextBox1.SelectionStart = curChar; //nastavim kurzor na povodnu hodnotu
                    textRichTextBox1.SelectionColor = Color.Black; //nastavim farbu na ciernu      
                }
            }
        }

        //zafarbi slova na cierno v riadku
        private void eraseHighlight(int curChar, int line)
        {
            if (line >= 0 && line < textRichTextBox1.Lines.Length)
            {
                int fisrtLineChar = textRichTextBox1.GetFirstCharIndexFromLine(line);

                textRichTextBox1.Select(fisrtLineChar, textRichTextBox1.Lines[line].Length);
                textRichTextBox1.SelectionColor = Color.Black;
                textRichTextBox1.SelectionLength = 0;
                textRichTextBox1.SelectionStart = curChar;
            }
        }

        //zmaze cele highlightovanie 
        private void eraseHighlightAll()
        {
            int curChar = textRichTextBox1.SelectionStart;

            textRichTextBox1.Select(0, textRichTextBox1.TextLength);
            textRichTextBox1.SelectionColor = Color.Black;
            textRichTextBox1.SelectionLength = 0;

            textRichTextBox1.SelectionStart = curChar;
        }

        //zvyraznovanie syntaxe
        public void highlightSyntax(bool checkPreviousLine, int line)
        {
            try
            {
                LockWindowUpdate(textRichTextBox1.Handle);

                //ak je povolene riadkove highlightovanie
                if (highLightLock != true)
                {
                    int curChar = textRichTextBox1.SelectionStart;
                    int curLine = 0;
                    //curLine priradim na akt. hodnotu z kurzora iba ak som si neposlal hodnotu externe (pri otvoreni suboru postupne prejdem vsetky riadky)
                    if (line == -1)
                    {
                        curLine = textRichTextBox1.GetLineFromCharIndex(curChar);
                    }
                    else
                    {
                        curLine = line;
                    }
                    int fisrtLineChar = textRichTextBox1.GetFirstCharIndexFromLine(curLine);


                    //zaciernit vsetky slova v riadku
                    eraseHighlight(curChar, curLine);


                    //ak je povolene higlightovanie danej skupiny, tak to vykonaj
                    if (hlCisla == true)
                    {
                        matchRegexp(Color.DarkOrange, cisla, curChar, curLine);
                    }
                    /*
                    if (hlZnaky == true)
                    {
                        matchRegexp(Color.MidnightBlue, znaky, curChar, curLine);
                    }
                    */
                    if (hlSkupina1 == true)
                    {
                        matchRegexp(Color.Blue, skupina1, curChar, curLine);
                    }
                    if (hlSkupina2 == true)
                    {
                        matchRegexp(Color.Sienna, skupina2, curChar, curLine);
                    }
                    if (hlSkupina3 == true)
                    {
                        matchRegexp(Color.DarkBlue, skupina3, curChar, curLine);
                    }
                    if (hlSkupina4 == true)
                    {
                        matchRegexp(Color.LightBlue, skupina4, curChar, curLine);
                    }
                    if (hlSkupina5 == true)
                    {
                        matchRegexp(Color.DarkRed, skupina5, curChar, curLine);
                    }
                    if (hlSkupina6 == true)
                    {
                        matchRegexp(Color.DarkViolet, skupina6, curChar, curLine);
                    }
                    if (hlRiadkovyKomentar == true)
                    {
                        matchRegexp(Color.Green, riadkovyKomentar, curChar, curLine);
                    }

                    /*
                    matchRegexp(Color.DarkOrange, cisla, curChar, curLine);
                    //matchRegexp(Color.MidnightBlue, znaky, curChar, curLine);
                    matchRegexp(Color.Blue, skupina1, curChar, curLine);
                    matchRegexp(Color.Sienna, skupina2, curChar, curLine);
                    matchRegexp(Color.DarkBlue, skupina3, curChar, curLine);
                    matchRegexp(Color.LightBlue, skupina4, curChar, curLine);
                    matchRegexp(Color.DarkRed, skupina5, curChar, curLine);
                    matchRegexp(Color.DarkViolet, skupina6, curChar, curLine);
                    matchRegexp(Color.Green, riadkovyKomentar, curChar, curLine);
                    */

                    //ak existuje o riadok menej, tak ho skontrolujem (osetreny pripad ked dam enter, tak by sa mi nemuselo zvyraznit slovo)
                    if (curLine > 0 && checkPreviousLine)
                    {
                        eraseHighlight(curChar, curLine - 1);

                        if (hlCisla == true)
                        {
                            matchRegexp(Color.DarkOrange, cisla, curChar, curLine - 1);
                        }
                        /*
                        if (hlZnaky == true)
                        {
                            matchRegexp(Color.MidnightBlue, znaky, curChar, curLine - 1);
                        }
                        */
                        if (hlSkupina1 == true)
                        {
                            matchRegexp(Color.Blue, skupina1, curChar, curLine - 1);
                        }
                        if (hlSkupina2 == true)
                        {
                            matchRegexp(Color.Sienna, skupina2, curChar, curLine - 1);
                        }
                        if (hlSkupina3 == true)
                        {
                            matchRegexp(Color.DarkBlue, skupina3, curChar, curLine - 1);
                        }
                        if (hlSkupina4 == true)
                        {
                            matchRegexp(Color.LightBlue, skupina4, curChar, curLine - 1);
                        }
                        if (hlSkupina5 == true)
                        {
                            matchRegexp(Color.DarkRed, skupina5, curChar, curLine - 1);
                        }
                        if (hlSkupina6 == true)
                        {
                            matchRegexp(Color.DarkViolet, skupina6, curChar, curLine - 1);
                        }
                        if (hlRiadkovyKomentar == true)
                        {
                            matchRegexp(Color.Green, riadkovyKomentar, curChar, curLine - 1);
                        }

                        /*
                        matchRegexp(Color.DarkOrange, cisla, curChar, curLine - 1);
                        //matchRegexp(Color.MidnightBlue, znaky, curChar, curLine - 1);
                        matchRegexp(Color.Blue, skupina1, curChar, curLine - 1);
                        matchRegexp(Color.Sienna, skupina2, curChar, curLine - 1);
                        matchRegexp(Color.DarkBlue, skupina3, curChar, curLine - 1);
                        matchRegexp(Color.LightBlue, skupina4, curChar, curLine - 1);
                        matchRegexp(Color.DarkRed, skupina5, curChar, curLine - 1);
                        matchRegexp(Color.DarkViolet, skupina6, curChar, curLine - 1);
                        matchRegexp(Color.Green, riadkovyKomentar, curChar, curLine - 1);
                        */
                    }
                }
            }
            finally
            {
                LockWindowUpdate(IntPtr.Zero);
            }
        }

        //spusti sa pri otvoreni suboru - highlightne cely subor
        public void highLightSyntaxOnOpen()
        {
            //var start = DateTime.Now;
            try
            {
                LockWindowUpdate(textRichTextBox1.Handle);

                //pre zvyraznenie textu vytvorim docasny richtextbox + priradim mu rovnaky font
                RichTextBox temp = new RichTextBox();
                temp.Font = textRichTextBox1.Font;

                //skopirujem do neho text z rtb
                temp.Text = textRichTextBox1.Text;

                //pri kazdom highglitnuti sa spusti handler, ze sa zmenil text - zakazem riadkovemu highlighteru aby vykonaval svoje operacie
                highLightLock = true;

                int curChar = textRichTextBox1.SelectionStart;

                //MessageBox.Show("OnOpen\nhlCisla="+hlCisla.ToString()+"\n"+language);

                //otestovat riadok proti regexp - postupne kazdu skupinu
                if (hlCisla == true)
                {
                    foreach (Match klucoveSlovo in cisla.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.DarkOrange; //zafarbim najdene slovo
                    }
                }

                /*
                if (hlZnaky == true)
                {
                    foreach (Match klucoveSlovo in znaky.Matches(textRichTextBox1.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        textRichTextBox1.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        textRichTextBox1.SelectionColor = Color.MidnightBlue; //zafarbim najdene slovo
                    }
                }
                */

                if (hlSkupina1 == true)
                {
                    foreach (Match klucoveSlovo in skupina1.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.Blue; //zafarbim najdene slovo
                    }
                }

                if (hlSkupina2 == true)
                {
                    foreach (Match klucoveSlovo in skupina2.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.Sienna; //zafarbim najdene slovo
                    }
                }

                if (hlSkupina3 == true)
                {
                    foreach (Match klucoveSlovo in skupina3.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.DarkBlue; //zafarbim najdene slovo
                    }
                }

                if (hlSkupina4 == true)
                {
                    foreach (Match klucoveSlovo in skupina4.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.DarkRed; //zafarbim najdene slovo
                    }
                }

                if (hlSkupina5 == true)
                {
                    foreach (Match klucoveSlovo in skupina5.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.DarkRed; //zafarbim najdene slovo
                    }
                }

                if (hlSkupina6 == true)
                {
                    foreach (Match klucoveSlovo in skupina6.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index+fisrtLineChar -pozicia slova v riadku + znaky pred danym riadkom
                        temp.Select(klucoveSlovo.Index, klucoveSlovo.Length);
                        temp.SelectionColor = Color.DarkViolet; //zafarbim najdene slovo
                    }
                }

                //otestovat riadok proti regexp - pre riadkovyKomentar
                if (hlRiadkovyKomentar == true)
                {
                    foreach (Match klucoveSlovo in riadkovyKomentar.Matches(temp.Text))
                    {
                        //klucoveSlovo.Index + fisrtLineChar ->pozicia slova v riadku + znaky pred danym riadkom
                        //textRichTextBox1.Lines[line].Length - klucoveSlovo.Index ->pocet znakov od najdeneho slova po koniec riadku
                        temp.Select(klucoveSlovo.Index, textRichTextBox1.Lines[textRichTextBox1.GetLineFromCharIndex(klucoveSlovo.Index)].Length - klucoveSlovo.Index + textRichTextBox1.GetFirstCharIndexFromLine(textRichTextBox1.GetLineFromCharIndex(klucoveSlovo.Index)));
                        temp.SelectionColor = Color.Green; //zafarbim najdene slovo
                    }
                }

                //skopirujem naspat zvyrazneny text do viditelneho rtb
                textRichTextBox1.Rtf = temp.Rtf;

                textRichTextBox1.SelectionStart = curChar;
                textRichTextBox1.SelectionLength = 0; //deselectnem
                textRichTextBox1.SelectionColor = Color.Black; //nastavim farbu na ciernu
                /*
                if (hlRiadkovyKomentar == true)
                {
                    foreach (Match klucoveSlovo in riadkovyKomentar.Matches(textRichTextBox1.Text))
                    {
                        //klucoveSlovo.Index + fisrtLineChar ->pozicia slova v riadku + znaky pred danym riadkom
                        //textRichTextBox1.Lines[line].Length - klucoveSlovo.Index ->pocet znakov od najdeneho slova po koniec riadku
                        textRichTextBox1.Select(klucoveSlovo.Index, textRichTextBox1.Lines[textRichTextBox1.GetLineFromCharIndex(klucoveSlovo.Index)].Length - klucoveSlovo.Index + textRichTextBox1.GetFirstCharIndexFromLine(textRichTextBox1.GetLineFromCharIndex(klucoveSlovo.Index)));
                        textRichTextBox1.SelectionColor = Color.Green; //zafarbim najdene slovo
                  
                      MessageBox.Show("klucoveSlovo.Index = " + klucoveSlovo.Index + "\n"+
                          "temp.Lines[temp.GetLineFromCharIndex(klucoveSlovo.Index)].Length = " + textRichTextBox1.Lines[textRichTextBox1.GetLineFromCharIndex(klucoveSlovo.Index)].Length + "\n" +
                          "temp.GetFirstCharIndexFromLine(temp.GetLineFromCharIndex(klucoveSlovo.Index)) = " + textRichTextBox1.GetFirstCharIndexFromLine(textRichTextBox1.GetLineFromCharIndex(klucoveSlovo.Index)) + "\n");
                    
                    }
                }

                textRichTextBox1.SelectionStart = 0;
                textRichTextBox1.SelectionLength = 0; //deselectnem
                textRichTextBox1.SelectionColor = Color.Black; //nastavim farbu na ciernu
                */
                highLightLock = false;
            }
            finally
            {
                LockWindowUpdate(IntPtr.Zero);
            }
            /*
            var stop = DateTime.Now;
            var diff = stop - start;
            MessageBox.Show(diff.ToString());
            */
        }

        //suma vsetkych riadkov a znakov + aktualna pozicia carretu do status baru
        private void textStatus_update()
        {
            //ziskat poziciu aktualneho riadku (z pozicie kurzora)
            int curChar = textRichTextBox1.SelectionStart;
            int curLine = textRichTextBox1.GetLineFromCharIndex(curChar);

            int totalCurChar = curChar;
            //ziskat poziciu aktualneho znaku v riadku- v curChar je teraz index znaku v celom texte
            //suradnice akt. bodu
            Point curPt = textRichTextBox1.GetPositionFromCharIndex(curChar);
            //suradnice prveho znaku riadku
            curPt.X = 0;
            //index prveho znaku riadku v celom texte
            int fisrtLineChar = textRichTextBox1.GetCharIndexFromPosition(curPt);

            //opdpoicitam index akt. znaku a index prveho znaku v riadku -> dostanem index akt. znaku v riadku
            if (textRichTextBox1.Lines.Length == curLine + 1 && textRichTextBox1.Lines[curLine] == "")
            {
                curChar = curChar - fisrtLineChar;
            }
            else
            {
                curChar = curChar - fisrtLineChar + 1;
            }
            curLine++;


            //prepisat status bar
            textLabelStatus.Text = "Sumár:   " +
                                    textRichTextBox1.Lines.Length.ToString() + " r.    " +
                                    textRichTextBox1.TextLength.ToString() + " z.    " +
                                    "|    Aktuálny:    " +
                                    curLine.ToString() + ". r.    " +
                                    curChar.ToString() + ". z.    ";
        }

        //zvyraznenie aktualneho riadku
        private void textLineHighlight_update()
        {
            //najst poziciu prveho bodu v akt. riadku
            int curChar = textRichTextBox1.SelectionStart;
            Point curPt = textRichTextBox1.GetPositionFromCharIndex(curChar);
            curPt.X = 0;

            //posunut highilght up a down label na spravne pozicie
            textHighlightUp.Location = curPt;
            curPt.Y += textRichTextBox1.Font.Height;
            textHighlightDown.Location = curPt;
        }

        //vzdy ked sa zmeni pozicia carretu (textový kurzor), tak sa prepise status bar (okrem selekcie textu)
        //+highlitne sa akt. riadok
        private void textRichTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (textRichTextBox1.SelectionLength == 0)
            {
                textStatus_update();
                textLineHighlight_update();
            }
        }

        /*PREBRATE A UPREVENE Z http://www.codeproject.com/KB/edit/numberedtextbox.aspx
           zaciatok*/

        //aktualizovat cislovanie riadkov
        private void textNumbering_update()
        {
            Point pos = new Point(0, 0);
            int firstIndex = textRichTextBox1.GetCharIndexFromPosition(pos);
            int firstLine = textRichTextBox1.GetLineFromCharIndex(firstIndex);

            //now we get index of last visible char and number of last visible line
            pos.X = textRichTextBox1.ClientRectangle.Width;
            pos.Y = textRichTextBox1.ClientRectangle.Height;
            int lastIndex = textRichTextBox1.GetCharIndexFromPosition(pos);
            int lastLine = textRichTextBox1.GetLineFromCharIndex(lastIndex);

            //this is point position of last visible char, we'll use its Y value for calculating numberLabel size
            pos = textRichTextBox1.GetPositionFromCharIndex(lastIndex);


            //finally, renumber label
            textNumbering.Text = "";
            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                textNumbering.Text += i + 1 + " \n";
            }

            /*
            //cislovanie riadkov
            textNumbering.Text = "";
            for (int i = 1; i <= textRichTextBox1.Lines.Length; i++)
            {
                textNumbering.Text += i + " \n";
            }
            */
        }

        //ak stlacim enter, tak sa skontroluje syntax aj predchadzajuceho riadku
        private void textRichTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //true = kontrola aj predchadzajuceho riadku
                highlightSyntax(true, -1);
            }
        }

        //ak nastala zmena textu - updatuje sa numbering a highlight syntaxe
        private void textRichTextBox1_TextChanged(object sender, EventArgs e)
        {
            textNumbering_update();
            highlightSyntax(false, -1);
        }

        //numbering sa musi memit podla scrollovania
        //+ musi sa zmenit poloha highlightera riadku
        private void textRichTextBox1_VScroll(object sender, EventArgs e)
        {
            //move location of numberLabel for amount of pixels caused by scrollbar
            int d = textRichTextBox1.GetPositionFromCharIndex(0).Y % (textRichTextBox1.Font.Height + 1);
            textNumbering.Location = new Point(0, d);

            textNumbering_update();
            textLineHighlight_update();
        }

        //ak nastala zmena velkosti okna
        private void textRichTextBox1_Resize(object sender, EventArgs e)
        {
            textRichTextBox1_VScroll(null, null);

            //prisposobit sirku zvyraznenia riadku
            textHighlightUp.Width = textRichTextBox1.Width;
            textHighlightDown.Width = textRichTextBox1.Width;
        }

        //ak sa zmeni font
        private void textRichTextBox1_FontChanged(object sender, EventArgs e)
        {
            textNumbering_update();
            textRichTextBox1_VScroll(null, null);
        }
        /*PREBRATE A UPREVENE Z http://www.codeproject.com/KB/edit/numberedtextbox.aspx
            koniec*/


        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;

            //MessageBox.Show(senderComboBox.SelectedItem.ToString());

            if (senderComboBox.SelectedItem.ToString() != language)
            {
                language = senderComboBox.SelectedItem.ToString();
                initRegex();
                eraseHighlightAll();
                highLightSyntaxOnOpen();
            }
        }
    }

    //do objektov tejto triedy sa ulozia informacie z xml pri vytvoreni numbered rtb
    public class syntaxLang
    {
        public string language;
        public string extension;
        public List<string> keywords;
        public string riadkovyKomentar;
        public string komentarStart;
        public string komentarKoniec;

        public syntaxLang()
        {
            language = "txt";
            keywords = new List<string>();
        }
    }
}
