using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.Globalization;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.Services;
using DevExpress.XtraRichEdit.Menu;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.Forms;
using DevExpress.XtraPrinting.Preview;
using static prot3.Form2;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraSpellChecker;
using AsistenteProtocolario.Properties;

namespace prot3
{
  /*  public class compareciente
    {
        public string nombre;
        public string id;
    }*/

    public partial class FormPrincipal : RibbonForm
    {
        public static string[] comparecientes = new string[300];
        public int contadorComparecientes = 0;
        public string nombreNotario = "";
        public string numeroEscritura = "";
        public string fechaEscritura = "";
        public string lugarEscritura = "";

        //public static compareciente[] Comparecientes = new compareciente[20];
        //public bool UseCustomPaste { get { return cbPastePlainText.Checked; } }
        public int numeroDePaginas=1,secciones=1,cantidadLineas=1;

        //public virtual DocumentCapability documentCap { get; set; }
        public float margenGrande = 46.5f/*46.5f*/, margenMenor = 19.5f, margenTop = 78f, margenBottom = 40f;//1.75f;
        public bool formatoInverso = false;
        
        public FormPrincipal()
        {
            WindowState = FormWindowState.Maximized;
            CultureInfo culture_info = new CultureInfo("es-ES");
            InitializeComponent();
            InitSkinGallery();
            InitializeRichEditControl();
            ribbonControl.SelectedPage = fileRibbonPage1;
            richEditControl.Options.SpellChecker.AutoDetectDocumentCulture = false;            
            spellChecker.Culture = culture_info;
            arreglarLabels();
            conversiones.Text = "Conversiones";
            impresionRapida.Visibility = BarItemVisibility.Never;
            richEditControl.LayoutUnit = DocumentLayoutUnit.Document;
            CustomRichEditCommandFactoryService commandFactory = new CustomRichEditCommandFactoryService(richEditControl, richEditControl.GetService<IRichEditCommandFactoryService>());
            richEditControl.RemoveService(typeof(IRichEditCommandFactoryService));
            richEditControl.AddService(typeof(IRichEditCommandFactoryService), commandFactory);
            foreach (string key in escriturasPrecarcadas.escriturasEncabezado.Keys)
            {
                repositoryItemComboBox.Items.Add(key);
            }
            foreach (string key in escriturasPrecarcadas.escriturasSinEncabezado.Keys)
            {
                repositoryItemComboBox.Items.Add(key);
            }
            // documentCap = DocumentCapability.Hidden;            
            // StartType = SectionStartType.NextPage;
            //LoadOpenOfficeDictionaries();
            /*Console.WriteLine("----------------");
            Console.WriteLine(spellChecker.Dictionaries[0].DictionaryPath);
            Console.WriteLine(spellChecker.Dictionaries[0].Culture);
            Console.WriteLine(spellChecker.Dictionaries[0].AlphabetChars);
            Console.WriteLine(spellChecker.Dictionaries[0].Loaded);
            Console.WriteLine(spellChecker.Dictionaries[0].WordCount);*/
        }
       /* private void LoadOpenOfficeDictionaries()
        {
            OptionsSpelling optionsSpelling1 = new OptionsSpelling();
            this.spellChecker.SetSpellCheckerOptions(this.richEditControl, optionsSpelling1);
            //spellChecker.Dictionaries.Clear();

            this.spellChecker.Culture = Settings.Default.Español;
            SpellCheckerISpellDictionary spellCheckerOpenOfficeDictionary1 = new SpellCheckerISpellDictionary();
            this.spellChecker.CheckAsYouTypeOptions.Color = System.Drawing.Color.Brown;
            this.spellChecker.Culture = AsistenteProtocolario.Properties.Settings.Default.Español;
            spellCheckerOpenOfficeDictionary1.AlphabetPath = "";
            spellCheckerOpenOfficeDictionary1.CacheKey = null;
            spellCheckerOpenOfficeDictionary1.Culture = new System.Globalization.CultureInfo("es-ES");
            spellCheckerOpenOfficeDictionary1.DictionaryPath = "C:\\Users\\FernandoHanz\\source\\repos\\prot3\\prot3\\dicts\\es_ES\\es_ES.dic";//".\\dicts\\es_ES\\es_ES.dic"; //Resources.es_ES1.// 
            spellCheckerOpenOfficeDictionary1.Encoding = System.Text.Encoding.GetEncoding(28591);
            spellCheckerOpenOfficeDictionary1.GrammarPath = "C:\\Users\\FernandoHanz\\source\\repos\\prot3\\prot3\\dicts\\es_ES\\es_ES.aff";//".\\dicts\\es_ES\\es_ES.aff";//Resources.es_ES.ToString();//
            spellCheckerOpenOfficeDictionary1.Load();

            this.spellChecker.Dictionaries.Add(spellCheckerOpenOfficeDictionary1);
            this.spellChecker.LevenshteinDistance = 4;
            this.spellChecker.ParentContainer = null;
            this.spellChecker.SpellCheckMode = DevExpress.XtraSpellChecker.SpellCheckMode.AsYouType;
            this.spellChecker.SpellingFormType = DevExpress.XtraSpellChecker.SpellingFormType.Word;
           
        }*/

        /// <summary>
        /// ////////
        /// 
        public class CustomRichEditCommandFactoryService : IRichEditCommandFactoryService
        {
            readonly IRichEditCommandFactoryService service;
            readonly RichEditControl control;

            public CustomRichEditCommandFactoryService(RichEditControl control, IRichEditCommandFactoryService service)
            {
                DevExpress.Utils.Guard.ArgumentNotNull(control, "control");
                DevExpress.Utils.Guard.ArgumentNotNull(service, "service");
                this.control = control;
                this.service = service;
            }

            public RichEditCommand CreateCommand(RichEditCommandId id)
            {
              
                if (id == RichEditCommandId.PasteSelection )
                    return new CustomPasteSelectionCommand(control);

                return service.CreateCommand(id);
            }
        }
        /// //////////////
        /// </summary>
        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }
        void InitializeRichEditControl()
        {

            formatearTexto();
        }
        public void formatoParrafo()
        {
            ParagraphProperties  pp = richEditControl.Document.BeginUpdateParagraphs(richEditControl.Document.Range);
            pp.SpacingAfter = 0;
            pp.SpacingBefore = 0;
            pp.Alignment = ParagraphAlignment.Justify;
            pp.KeepLinesTogether = false;
            pp.LineSpacingType = ParagraphLineSpacing.Exactly;
            pp.LineSpacing = 8.5f;//4.955f;
            richEditControl.Document.EndUpdateParagraphs(pp);

        }
        void formatearTexto()
        {
            richEditControl.Options.DocumentCapabilities.Undo = DocumentCapability.Disabled;
            richEditControl.Document.Unit = DevExpress.Office.DocumentUnit.Millimeter;
            formatoParrafo();
            richEditControl.Document.DefaultCharacterProperties.FontName = "Calibri";
            richEditControl.Document.DefaultCharacterProperties.FontSize = 11;
            richEditControl.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.Folio;
            richEditControl.Document.Sections[0].Page.Landscape = false;
            richEditControl.Document.Sections[0].Margins.Left = margenGrande;
            richEditControl.Document.Sections[0].Margins.Right = margenMenor;
            richEditControl.Document.Sections[0].Margins.Top = margenTop; 
            richEditControl.Document.Sections[0].Margins.Bottom = margenBottom; 
            //     numeroDePaginas = richEditControl.Document.Sections.Count;
            /* richEditControl.Document.Sections[1].Page.Landscape = false;
               richEditControl.Document.Sections[1].Margins.Top = 88.2f;
               richEditControl.Document.Sections[1].Margins.Left = margenMenor; 
               richEditControl.Document.Sections[1].Margins.Right = margenGrande;
               richEditControl.Document.Sections[1].Margins.Bottom = 45f;*/
            richEditControl.Document.Sections[0].StartType = SectionStartType.NextPage;
         //   Console.WriteLine(documentCap);
            Console.WriteLine(richEditControl);
            richEditControl.Options.DocumentCapabilities.Undo = DocumentCapability.Enabled;
        }


        private void insertPageNumberItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        private void CuerdasAMetros()
        {
            string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            if (seleccion == null)
            {
                MessageBox.Show("Seleccione un valor de la hoja.");
                return;
            }
            {
                bool numero = Double.TryParse(seleccion, out Double num);
                if (!numero)
                {
                    MessageBox.Show("El valor no es un numero");
                    return;
                }

                num *= 20.90;
                richEditControl.Document.Replace(richEditControl.Document.Selection, num.ToString());
            }
        }
        private void Cuerdas2AMetros2()
        {
            string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            if (seleccion == null)
            {
                MessageBox.Show("Seleccione un valor de la hoja.");
                return;
            }
            {
                bool numero = Double.TryParse(seleccion, out Double num);
                if (!numero)
                {
                    MessageBox.Show("El valor no es un numero");
                    return;
                }

                num *= 437; 
               // num *= 3930.397; verificar el valor de una cuerda 
                richEditControl.Document.Replace(richEditControl.Document.Selection, num.ToString());
            }
        }
        private void intToStr_ItemClick(object sender, ItemClickEventArgs e)
        {
            string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            Console.WriteLine(seleccion);
            if (seleccion == "")
            {
                MessageBox.Show("Seleccione un valor de la hoja.");
                return;
            }
            double num;
            // string seleccion = rtbEditor.Document.Selection.ToString();
            try

            {
                //num = Convert.ToDouble(seleccion, CultureInfo.InvariantCulture);
                num = Double.Parse(seleccion.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("El valor no es un entero");
                return;
            }
           /*bool numero = double.TryParse(seleccion, out double num);
            if (!numero)
            {
                MessageBox.Show("El valor no es un entero");
                return;
            }*/
            if (num > 999999999999)
            {
                MessageBox.Show("El número no puede ser mayor a 999,999,999,999");
                return;
            }
            if (num != 0)
            {
                string res = NumerosALetras.enletras(num);
                res = CorregirUnoUn(res);
                Console.WriteLine(res);
                richEditControl.Document.Replace(richEditControl.Document.Selection, res);
            }
        }
        public string CorregirUnoUn(string res)
        {
            while (res.Contains("UNO M"))
            {
                int val = res.IndexOf("UNO M");
                res = res.Remove(val + 2, 1);
                Console.WriteLine(res, val);
            }
            return res;
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            CuerdasAMetros();
        }
        private void cuerdas2am2BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cuerdas2AMetros2();
        }
        //insertar encabezado
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)//encabezado
        {
            Encabezado modal = new Encabezado();
            modal.Owner = this;
            modal.ShowDialog();
            
            Console.WriteLine(modal.DialogResult);
            if (modal.DialogResult == DialogResult.OK)
            {
                comparecientes[0] = modal.nombreCompareciente;
                contadorComparecientes = 1;
                nombreNotario = modal.nombreNotario;
                numeroEscritura = modal.noEscritura;
                fechaEscritura = modal.fecha;
                lugarEscritura = modal.nombreLugar;
                richEditControl.Document.BeginUpdate();
                richEditControl.Document.InsertText(richEditControl.Document.CaretPosition, modal.resultado);
                richEditControl.Document.EndUpdate();
            }            
        }
        private void insertarEncabezado()
        {
            Encabezado modal = new Encabezado();
            modal.Owner = this;
            modal.ShowDialog();

            Console.WriteLine(modal.DialogResult);
            if (modal.DialogResult == DialogResult.OK)
            {
                comparecientes[0] = modal.nombreCompareciente;
                contadorComparecientes = 1;
                nombreNotario = modal.nombreNotario;
                numeroEscritura = modal.noEscritura;
                fechaEscritura = modal.fecha;
                lugarEscritura = modal.nombreLugar;
                richEditControl.Document.BeginUpdate();
                richEditControl.Document.InsertText(richEditControl.Document.CaretPosition, modal.resultado);
                richEditControl.Document.EndUpdate();
            }
        }
        //insertar compareciente
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            agregarComp modal = new agregarComp();
           
            modal.ShowDialog();
            Console.WriteLine(modal.DialogResult);
            if (modal.DialogResult == DialogResult.OK)
            {
                comparecientes[contadorComparecientes] = modal.nombreCompareciente;
                contadorComparecientes++;
                richEditControl.Document.BeginUpdate();
                richEditControl.Document.InsertText(richEditControl.Document.CaretPosition, modal.resultado);
                richEditControl.Document.EndUpdate();

            }
        }
        private void richEditControl_PopupMenuShowing_1(object sender, DevExpress.XtraRichEdit.PopupMenuShowingEventArgs e)
        {
            e.Menu.RemoveMenuItem(RichEditCommandId.IncreaseIndent);
            e.Menu.RemoveMenuItem(RichEditCommandId.DecreaseIndent);
            e.Menu.RemoveMenuItem(RichEditCommandId.EditHyperlink);
            e.Menu.RemoveMenuItem(RichEditCommandId.CreateHyperlink);
            e.Menu.RemoveMenuItem(RichEditCommandId.NewComment);
            e.Menu.RemoveMenuItem(RichEditCommandId.InsertPicture);
            e.Menu.RemoveMenuItem(RichEditCommandId.CreateBookmark);
            e.Menu.RemoveMenuItem(RichEditCommandId.NextComment);
            e.Menu.RemoveMenuItem(RichEditCommandId.NewCommentContentMenu);
            e.Menu.RemoveMenuItem(RichEditCommandId.InsertParagraph);
            e.Menu.RemoveMenuItem(RichEditCommandId.ShowFontForm);
            e.Menu.RemoveMenuItem(RichEditCommandId.ShowParagraphForm);
            e.Menu.RemoveMenuItem(RichEditCommandId.ShowNumberingListForm);
            e.Menu.RemoveMenuItem(RichEditCommandId.AddWordToDictionary);
            //  e.Menu.Items.Clear();
            // e.Menu.EnableMenuItem(RichEditCommandId.IncreaseIndent);
            // Disable the "Hyperlink..." menu item:
            // e.Menu.DisableMenuItem(RichEditCommandId.CreateHyperlink);

            // Create a RichEdit command, which inserts an inline picture into a document:
            //IRichEditCommandFactoryService service = (IRichEditCommandFactoryService)richEditControl.GetService(typeof(IRichEditCommandFactoryService));
            // RichEditCommand cmd = service.CreateCommand(RichEditCommandId.InsertPicture);

            /*//Create a menu item for the new command:
            RichEditMenuItemCommandWinAdapter menuItemCommandAdapter = new RichEditMenuItemCommandWinAdapter(cmd);
            RichEditMenuItem menuItem = (RichEditMenuItem)menuItemCommandAdapter.CreateMenuItem(DevExpress.Utils.Menu.DXMenuItemPriority.Normal);
            menuItem.BeginGroup = true;
            e.Menu.Items.Add(menuItem);*/

            // Insert a new item into the Richedit popup menu and handle its click event:
            /*  RichEditMenuItem myItem = new RichEditMenuItem("Highlight Selection", new EventHandler(MyClickHandler));
              e.Menu.Items.Add(myItem);*/

        }
        private void richEditControl_DocumentLoaded(object sender, EventArgs e)
        {
                formatearTexto();            
        }
        private void richEditControl_RtfTextChanged(object sender, EventArgs e)
        {
            CustomLayoutVisitor visitor = new CustomLayoutVisitor(richEditControl.Document);
            //reiniciar la variable de conteo de lineas
            visitor.Reset();
            int pageCount = richEditControl.DocumentLayout.GetFormattedPageCount();
            for (int i = 0; i < pageCount; i++)
                visitor.Visit(richEditControl.DocumentLayout.GetPage(i));
            barStaticItem1.Caption = "Linea: " + visitor.RowIndex.ToString()+" de " + visitor.Lineas.ToString()+ "   Páginas: " + pageCount.ToString();


            if (CustomPasteSelectionCoreCommand.pegado)
            {
                formatoParrafo();
                CustomPasteSelectionCoreCommand.pegado = false;
                return;
            }

        }

        //visitor necesario para conteo de lineas
        public class CustomDocumentLayoutVisitor : LayoutVisitor
        {

            public int TextLinesCount = 0;
            public int actualLines = 0;
            public bool encontrado = false;
            DevExpress.XtraRichEdit.API.Native.Document document;
            protected override void VisitRow(LayoutRow row)
            {
                if (row.GetParentByType<LayoutTable>() == null && row.GetParentByType<LayoutFooter>() == null && row.GetParentByType<LayoutHeader>() == null)
                    { TextLinesCount++;
                    if (!encontrado)
                    {
                        actualLines++;
                        if (row.Range.Contains(document.CaretPosition.ToInt()))
                        {
                            encontrado = true;
                            actualLines = document.CaretPosition.ToInt() - row.Range.Start;
                        }
                    }

                }
                base.VisitRow(row);
            }
        }
        private void NuevaSeccion(DevExpress.XtraRichEdit.API.Native.Document document, DocumentLayout layout)

        {
            int paginaNueva = layout.GetPageCount();
            int secs = document.Sections.Count();
            Console.WriteLine("secciones: " + secs.ToString());
            //LayoutPage page = layout.GetPage(paginaNueva);
            //agregarle un rango
            //FixedRange frange = page.MainContentRange;
            //crear el rango 
            //DocumentRange range = document.CreateRange(frange.Start, frange.Length);
            //insertar seccion en ese rango al final de esta pagina
            // document.InsertSection(document.CaretPosition);
            
             LayoutPage page = layout.GetPage(paginaNueva-1);
             FixedRange frange = page.MainContentRange;
             DocumentRange range = document.CreateRange(frange.Start, frange.Length);
             document.InsertSection(range.End);

            //document.AppendSection();
            //Section section = document.Sections[paginaNueva-1];
            //SetMargins(section, paginaNueva);
        }

        private void formatoInvBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraRichEdit.API.Native.Document document = richEditControl.Document;
            formatoInverso = true;
            //NuevaSeccion(document, richEditControl.DocumentLayout);
            SplitToSections(document, richEditControl.DocumentLayout);
        }

        private void barButtonItem3_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                int lineas = Convert.ToInt16((lineasEnBlancoTxt.EditValue.ToString()));
                if (lineas > 24)
                {
                    MessageBox.Show("No se pueden agregar mas de 24 lineas en blanco. Si desea imprimir en el lado inverso del protocolo seleccione 'Formato Inv' en el cuadro de herramientas");
                    return;
                }
                else
                {
                    for (int i=0;i<lineas;i++)
                    {
                        richEditControl.Document.InsertText(richEditControl.Document.Range.Start, DevExpress.Office.Characters.LineBreak.ToString());
                    }
                }
            }
            catch
            {
                Console.WriteLine("err");
            }
            

        }

        private void lineasEnBlancoTxt_EditValueChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(lineasEnBlancoTxt.EditValue.ToString(), "[^0-9]"))
            {
                MessageBox.Show("Este campo es numérico únicamente.");
                lineasEnBlancoTxt.EditValue= limpiarNumeros(lineasEnBlancoTxt.EditValue.ToString());
            }
        }
        private string limpiarNumeros(string s)
        {
            Regex rxNonDigits = new Regex(@"[^\d]+");
            if (string.IsNullOrEmpty(s)) return s;
            string cleaned = rxNonDigits.Replace(s, "");
            return cleaned;
        }

        private void ivaBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            resImpuesto.EditValue = (Math.Round(Convert.ToDouble( valorTxt.EditValue.ToString())* .12 / 1.12,2)).ToString();
        }

        private void primeraVentaBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            resImpuesto.EditValue = Math.Round(Convert.ToDouble(valorTxt.EditValue.ToString()) * .12, 2).ToString();
        }

        private void segundaVentaBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            resImpuesto.EditValue = Math.Round(Convert.ToDouble(valorTxt.EditValue.ToString()) * .03, 2).ToString();
        }

        private void contadorFechaInicio_EditValueChanged(object sender, EventArgs e)
        {
         DateTime fecha=  Convert.ToDateTime(contadorFechaInicio.EditValue.ToString());
            Console.WriteLine(fecha.ToString());
            for (int i=0; i<25; )
            {              

                Console.WriteLine(fecha.DayOfWeek);
                if (!(fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday))
                { i++; }
                fecha = fecha.AddDays(1);
            }
            while ((fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday))
            {
                fecha = fecha.AddDays(1);
            }
            fechaResultado.Caption = "Fecha Resultante:  " + fecha.ToString("dddd dd/MM/yyyy");
            Console.WriteLine(fecha.ToString());
        }

        private void imprimirBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
          /*  PrintableComponentLink componentLink = new PrintableComponentLink(new PrintingSystem());
            componentLink.Component = richEditControl;
            componentLink.CreateDocument();
            PrintTool pt = new PrintTool(componentLink.PrintingSystemBase);
            pt.PrintDialog();*/
            
            /*  DefaultPrintDialogRunner PrintDialog = new DefaultPrintDialogRunner();
            IWin32Window own = this.Owner;
            PrintDocument doc =  richEditControl.DocumentLayout;
            PrintDialog.Run(richEditControl.Document,UserLookAndFeel.Default, own, PrintDialogAllowFlags.AllowAllPages);
            */

            //PrintDialog.ShowDialog();
           // PrintDialogRunner.Instance = new SystemPrintDialogRunner();

           PrintDialogRunner.Instance = new MyRunner();

             PrintableComponentLink componentLink = new PrintableComponentLink(new PrintingSystem());
             componentLink.Component = richEditControl;
             componentLink.CreateDocument();
             PrintTool pt = new PrintTool(componentLink.PrintingSystemBase);
               pt.PrintDialog();
        }

        private void richEditControl_HyperlinkFormShowing(object sender, HyperlinkFormShowingEventArgs e)
        {
            Console.WriteLine("por la gloria");
            HyperlinkForm form = new HyperlinkForm(e.ControllerParameters);
            e.DialogResult = form.ShowDialog();
            e.Handled = true;
        }

        private void esciturasComboBox_EditValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine(escriturasCombo.EditValue.ToString());
        }

        private void insertarEscritur_ItemClick(object sender, ItemClickEventArgs e)
        {
            string key;
            try {
                key = escriturasCombo.EditValue.ToString().ToUpper(); }
            catch {
                return;
            }

                Console.WriteLine(key);
            if (escriturasPrecarcadas.escriturasEncabezado.ContainsKey(key))
            {
                Encabezado modal = new Encabezado();
                modal.Owner = this;
                modal.ShowDialog();

                Console.WriteLine(modal.DialogResult);
                if (modal.DialogResult == DialogResult.OK)
                {
                    comparecientes[0] = modal.nombreCompareciente;
                    contadorComparecientes = 1;
                    nombreNotario = modal.nombreNotario;
                    numeroEscritura = modal.noEscritura;
                    fechaEscritura = modal.fecha;
                    lugarEscritura = modal.nombreLugar;
                    richEditControl.Document.BeginUpdate();
                    richEditControl.Document.InsertText(richEditControl.Document.CaretPosition, modal.resultado);
                    richEditControl.Document.EndUpdate();

                    string escritura = escriturasPrecarcadas.escriturasEncabezado[key];
                    richEditControl.Document.BeginUpdate();
                    richEditControl.Document.AppendText(escritura);
                    richEditControl.Document.EndUpdate();
                }
                /*string escritura = escriturasPrecarcadas.escriturasEncabezado[key];
                Encabezado modal = new Encabezado();
                modal.Owner = this;
                modal.ShowDialog();

                Console.WriteLine(modal.DialogResult);
                if (modal.DialogResult == DialogResult.OK)
                {
                    richEditControl.Document.BeginUpdate();
                    richEditControl.Document.AppendText(modal.resultado);
                    richEditControl.Document.AppendText(escritura);
                    richEditControl.Document.EndUpdate();
                }*/
            }
            else
            {
                string escritura = escriturasPrecarcadas.escriturasSinEncabezado[key];

                richEditControl.Document.BeginUpdate();
                richEditControl.Document.AppendText(escritura);
                richEditControl.Document.EndUpdate();
            }
            
        }
        //insertar testimonio especial
        private void barButtonItem3_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            Console.WriteLine(comparecientes.Length);
            string testimonioEspecial = " (fs)";
            for (int i = 0; i < comparecientes.Length; i++)
            {
                if (comparecientes[i] != null)
                {
                    Console.WriteLine(comparecientes[i].Substring(0, 9));
                    if (comparecientes[i].Substring(0, 9) == "[noFirma]")
                    {
                        Console.WriteLine(comparecientes[0].Length);
                        if (comparecientes[i].Length == 9)
                        {
                            Console.WriteLine("vacio");
                        }
                        else
                        {
                            testimonioEspecial += " Aparece la huella dactilar del pulgar derecho del señor " + comparecientes[i].Substring(9)+ ",";
                            Console.WriteLine(comparecientes[i].Substring(9));
                        }
                    }
                    else
                    {
                        testimonioEspecial += " Ilegible,";
                    }
                }
                else
                {
                    break;
                }
            }
            testimonioEspecial += " Ante Mi: " + nombreNotario + " aparece el sello del Notario_________________ ES TESTIMONIO ESPECIAL: De la escritura número: "
                                    + numeroEscritura + ". autorizada en esta ciudad ante mis oficios con fecha " + fechaEscritura + ", que para remitir a ARCHIVO GENERAL PARA PROTOCOLOS, le extiendo, sello y firmo en __ hojas de papel bond, debidamente confrontadas" +
                                    " con su original el día de hoy en mi presencia " + lugarEscritura + ", " + fechaEscritura; 
            Console.WriteLine(testimonioEspecial);
            richEditControl.Document.BeginUpdate();
            richEditControl.Document.AppendText(testimonioEspecial);
            richEditControl.Document.EndUpdate();
        }

        private void richEditControl_Click(object sender, EventArgs e)
        {

        }

        private void formatearBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraRichEdit.API.Native.Document document = richEditControl.Document;
            formatoInverso = false;
            //NuevaSeccion(document, richEditControl.DocumentLayout);
            SplitToSections(document, richEditControl.DocumentLayout);
        }

        private void SplitToSections(DevExpress.XtraRichEdit.API.Native.Document document, DocumentLayout layout)
        {
            richEditControl.Options.DocumentCapabilities.Undo = DocumentCapability.Disabled;
            document.SelectAll();
            string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            //   document.Copy();
            //string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            richEditControl.Document.Delete(richEditControl.Document.Range);
            //    document.Paste(DocumentFormat.PlainText);
            richEditControl.Document.Replace(richEditControl.Document.Selection, seleccion);
        //    formatoParrafo();
            richEditControl.Options.DocumentCapabilities.Undo = DocumentCapability.Enabled;
            int pageCount = layout.GetFormattedPageCount();
            //para cada pagina de 0 a N 
            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                //tomar la seccion respectiva
                Section section = document.Sections[pageIndex];
                //dar margenes
                SetMargins(section, pageIndex);
                //si el numero de pagina es distinto de N-1
                if (pageIndex != pageCount - 1)
                {
                    //tomar la pagina
                    LayoutPage page = layout.GetPage(pageIndex);
                    //agregarle un rango
                    FixedRange frange = page.MainContentRange;
                    //crear el rango 
                    DocumentRange range = document.CreateRange(frange.Start, frange.Length);
                    //insertar seccion en ese rango al final de esta pagina
                    string sl = richEditControl.Document.GetText(range);
                    //eliminacion de saltos de linea
                    if(sl[0]=='\r' && sl[1]=='\n' && pageIndex>0)//verificacion si el string tiene saltos de linea agregados por la insercion de seleccion
                    {
                        richEditControl.Document.Replace(range, sl.Substring(2));
                    }
                    document.InsertSection(range.End);
                }
            }
         //   Console.WriteLine(document.Sections.Count.ToString()+" Secciones");
        }

        private void barButtonItem3_ItemClick_3(object sender, ItemClickEventArgs e)
        {
            string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            if (seleccion == null)
            {
                MessageBox.Show("No existe selección");
                return;
            }
            
            MakeTextUpperCaseCommand command = new MakeTextUpperCaseCommand(richEditControl);
            command.Execute();
            
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            string seleccion = richEditControl.Document.GetText(richEditControl.Document.Selection);
            if (seleccion == null)
            {
                MessageBox.Show("No existe selección");
                return;
            }

            MakeTextLowerCaseCommand command = new MakeTextLowerCaseCommand(richEditControl);
            command.Execute();
        }

        private void barButtonItem3_ItemClick_4(object sender, ItemClickEventArgs e)
        {
           double res= Math.Ceiling(Convert.ToDouble(valorTxt.EditValue.ToString()) * .002);
            if (res > 300)
                res = 300;
            resImpuesto.EditValue = res.ToString();
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (richEditControl.Text.Length > 0)
            {                
                if (richEditControl.Modified)
                {
                    dialogForm dialog = new dialogForm("Advertencia", "Se detectaron Cambios sin Guardar", "Guardar", "No Guardar","Cancelar");
                    dialog.ShowDialog();
                    this.DialogResult = DialogResult.None;
                    if (dialog.DialogResult == DialogResult.No)
                    {
                        //e.Cancel = true;
                        //this.Close();
                    }
                    else if(dialog.DialogResult ==  DialogResult.Yes)
                    {
                       // e.Cancel = true;
                        richEditControl.SaveDocument();
                    }
                    else if(dialog.DialogResult == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    //e.Cancel = true;
                    //Console.WriteLine("no GUardado");
                }                    
                else
                {
                    Console.WriteLine("guardado");
                  //  this.Close();
                }
            }
          

            //  richEditControl.SaveDocument();
        }

        private void SetMargins(Section section, int pageIndex)
        {
            // Console.WriteLine("indice de pag: "+ pageIndex.ToString());
            if (formatoInverso)
            {
                if (pageIndex % 2 == 0)
                {
                    section.Margins.Left = margenMenor;
                    section.Margins.Right = margenGrande;
                }
                else
                {
                    section.Margins.Left = margenGrande;
                    section.Margins.Right = margenMenor;
                }
            }
            else
            {
                if (pageIndex % 2 == 0)
                {
                    section.Margins.Left = margenGrande;
                    section.Margins.Right = margenMenor;
                }
                else
                {
                    section.Margins.Left = margenMenor;
                    section.Margins.Right = margenGrande;
                }
            }
        }

        public class StaticsticsVisitor : DocumentVisitorBase
        {
            readonly StringBuilder buffer;

            public int WordCount { get; private set; }
            protected StringBuilder Buffer { get { return buffer; } }

            public StaticsticsVisitor()
            {
                WordCount = 0;
                this.buffer = new StringBuilder();
            }

            public override void Visit(DocumentText text)
            {
                Buffer.Append(text.Text);
            }

            public override void Visit(DocumentParagraphEnd paragraphEnd)
            {
                FinishParagraph();
            }

            void FinishParagraph()
            {
                string text = Buffer.ToString();
                this.WordCount += text.Split(new char[] { ' ', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
                Buffer.Length = 0;
            }
        }

        public class CustomLayoutVisitor : LayoutVisitor
        {
            DevExpress.XtraRichEdit.API.Native.Document document;

            public CustomLayoutVisitor(DevExpress.XtraRichEdit.API.Native.Document doc)
            {
                this.document = doc;
                RowIndex = 0;
                ColIndex = 0;
                Lineas = 0;
                IsFound = false;
            }

            protected override void VisitRow(LayoutRow row)
            {
                if (row.GetParentByType<LayoutTable>() == null && row.GetParentByType<LayoutFooter>() == null && row.GetParentByType<LayoutHeader>() == null)
                    {
                        Lineas++;
                        if (!IsFound)
                        {
                            RowIndex++;
                            if (row.Range.Contains(document.CaretPosition.ToInt()))
                            {
                                IsFound = true;
                                ColIndex = document.CaretPosition.ToInt() - row.Range.Start;
                            }
                        }
                    }
                    base.VisitRow(row);
                }

            public void Reset()
            {
                RowIndex = 0;
                ColIndex = 0;
                Lineas = 0;
            }
            public int Lineas { get; private set; }
            public int ColIndex { get; private set; }
            public int RowIndex { get; private set; }
            public bool IsFound { get; private set; }

        }
        public void arreglarLabels()
        {
            this.Text = "Asistente Protocolario";
            this.fileRibbonPage1.Text = "Archivo y Herramientas";
            this.textoyFormatoTab.Text = "Texto y Formato";
            testimonioEspBtn.Caption = "Testimonio Especial";
            escriturasPageGroup.Text = "Escrituras Precargadas";
            insertarEscritur.Caption = "Insertar Escritura";
            intToStr.Hint = "seleccione un número de la hoja y conviértalo a letras.";
            lineasEnBlancoBtn.Caption = "Lineas En Blanco";
            lineasEnBlancoTxt.Caption = "Agregar";            
            formatearBtn.Caption = "Lado 1";
            formatearBtn.Hint = "Dar formato normal al documento";
            formatoDosBtn.Caption = "Lado 2";
            formatoDosBtn.Hint = "Dar formato inverso al documento";
            extrasTab.Text = "Impuestos, Testimonios y medidas";
            valorTxt.Caption = "Valor        ";
            resImpuesto.Caption = "Resultado ";
            ivaBtn.Caption = "IVA";
            primeraVentaBtn.Caption = "Primera Venta";
            segundaVentaBtn.Caption = "Segunda Venta";
            contadorDias.Text = "Contador de 25 días  hábiles";
            contadorFechaInicio.Caption = "Seleccione una fecha";
            fechaResultado.Caption = "Fecha Resultante:";
            notarialBtn.Caption = "Notarial";
            contadorDias.Text = "Fecha de vencimiento de testimonio especial";
            calculadoraGroup.Text = "Calculadoras";
            ribbonComun.Text = "Archivo";
            abrir.Caption = "Abrir";
            deshacer.Caption = "Deshacer";
            rehacer.Caption = "Rehacer";
            nuevo.Caption = "Nuevo";
            guardar.Caption = "Guardar";
            guardarComo.Caption = "Guardar Como";
            impresionRapida.Caption = "Impresion Rápida";
            imprimirBtn.Caption = "Imprimir";
            textoyFormatoTab.Text = "Texto y formato";
            clipboardRibbonPageGroup1.Text = "Portapapeles";
            pasteItem1.Caption = "Pegar";
            cutItem1.Caption = "Cortar";
            copyItem1.Caption = "Copiar";

        }
    }

}