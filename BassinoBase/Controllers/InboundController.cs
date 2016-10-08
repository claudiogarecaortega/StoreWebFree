using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using BassinoBase.Hubs;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Commodity;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Clients;
using Domain.Misc;
using Domain.Products;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Rotativa;

namespace BassinoBase.Controllers
{
    public class BinaryContentResult : ActionResult
    {
        private string ContentType;
        private byte[] ContentBytes;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            this.ContentBytes = contentBytes;
            this.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = this.ContentType;

            var stream = new MemoryStream(this.ContentBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }
    }
    public class InboundController : AbmController<Inbound, InboundViewModel, InboundViewModel>
    {
        private readonly IUserdDao _usersDao;
        private readonly IUserViewModelMapper _usersVmm;
        private readonly IClientDao _clientsDao;
        private readonly IMeasureUnitDao _measureDao;
        private readonly IPackageTypeDao _packageDao;
        private readonly IProductDao _productsDao;
        private readonly IBillDao _billDao;
        private readonly IBillTypeDao _billTypeDao;
        private readonly IInboundDao _inboundDao;
        private readonly IInboundViewModelMapper _iViewmodelMapper;
        private readonly INotificationDao _notificacionDao;
        private readonly IShipmentDao _shipDao;
        private readonly ICarDao _camionesDao;

        public InboundController(IInboundViewModelMapper inboundViewModelMapper, IInboundDao inboundDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IUserdDao usersDao, IClientDao clientsDao, IMeasureUnitDao measureDao, IPackageTypeDao packageDao, IProductDao productsDao, IBillDao billDao, IBillTypeDao billTypeDao, IInboundDao inboundDao1, IInboundViewModelMapper iViewmodelMapper, INotificationDao notificacionDao, IShipmentDao shipDao, ICarDao camionesDao, IUserViewModelMapper usersVmm)
            : base(abmControllerBahavior, inboundDao, inboundViewModelMapper, unitOfWorkHelper)
        {
            _usersDao = usersDao;
            _clientsDao = clientsDao;
            _measureDao = measureDao;
            _packageDao = packageDao;
            _productsDao = productsDao;
            _billDao = billDao;
            _billTypeDao = billTypeDao;
            _inboundDao = inboundDao1;
            _iViewmodelMapper = iViewmodelMapper;
            _notificacionDao = notificacionDao;
            _shipDao = shipDao;
            _camionesDao = camionesDao;
            _usersVmm = usersVmm;
        }
        public override void BeforeDelete(Inbound model)
        {
            model.IsDelete = true;
            base.BeforeDelete(model);
        }

        public ActionResult Restore(int id)
        {
            var activa = true;
            var model = _inboundDao.Get(id);
            if (model.IsDelivered)
                activa = false;
            ViewBag.activa = activa;


            string name = Request.Params["data"];
            return base.Details(Convert.ToInt32(id));
        }
        [HttpPost]
        public ActionResult Restore(InboundViewModel viewModel)
        {
            var model = _inboundDao.Get(viewModel.Id);
            model.IsUsed = false;
            //model.Envio = _shipDao.Get(0);
            var env = _shipDao.Get(model.Envio.Id);
            env.Cargars.Remove(model);
            env.TotalKilos = env.TotalKilos - Convert.ToDecimal(model.Kilos.Replace('.', ','));
            env.TotalPakages = env.TotalPakages - 1;
            _shipDao.Save();
            _inboundDao.Save();
            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModalShowData("La carga Numero: " + model.Secuencia + " ha sido devuelta al Deposito"));

        }
        public override ActionResult GridInfo(DataSourceRequest request, string filtro)
        {
            var result = DAO.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GridInfoCargasRestore(DataSourceRequest request, string filtro)
        {
            var result = _inboundDao.GetAllQRestore(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GridInfoCargas(DataSourceRequest request, string idViaje, string filtro, bool viaje, bool end, bool init)
        {
            if (filtro == "" && !viaje && !end && !init)
            {
                viaje = false;
                end = false;
                init = false;
            }
            var result = _inboundDao.GetAllQLess(idViaje, filtro, viaje, end, init).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GridInfoCrearViaje(DataSourceRequest request, string idViaje)
        {
            var result = _inboundDao.GetAllQPlus(idViaje).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GridInfoShip(DataSourceRequest request, string filtro, bool viaje, bool end, bool init)
        {
            if (!viaje && !end && !init)
            {
                viaje = true;
                end = true;
                init = true;
            }
            var result = _inboundDao.GetAllQFiltros(filtro, viaje, end, init).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public override ActionResult Create()
        {
            ViewBag.Users =_usersVmm.Map(_usersDao.GetAll());
            ViewBag.Clients = _clientsDao.GetAllClients();
            ViewBag.ClientsDestino = _clientsDao.GetAllDestinoList();
            ViewBag.ClientsOrigen = _clientsDao.GetAllOrigenList();
            ViewBag.Measures = _measureDao.GetAll();
            ViewBag.Package = _packageDao.GetAll();
            ViewBag.Products = _productsDao.GetAllActive();
            ViewBag.Bills = _billDao.GetAll();
            ViewBag.BillsType = _billTypeDao.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "RecepcionMercaderia");
                if (module != null)
                {
                    ViewBag.Add = module.Actions.Any(s => s.Description == "AgregarCuenta");
                }
                else
                {
                    ViewBag.Add = false;

                }
            }

            return base.Create();
        }
        private int CreateNotificationClient(Inbound Model, bool esDesconocido = false)
        {
            var model = _notificacionDao.Create();
            model.IdProduct = Model.Id;
            var user = UserManager.FindById(User.Identity.GetUserId());
            //
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Url = "/" + controllerName + "/Details/" + model.IdProduct;
            model.IsForAllUSers = false;
            model.Importance = esDesconocido ? "4" : "2";
            var action = "";
            if (esDesconocido)
            {
                if (actionName.Contains("Create"))
                    action = "hubo un ingreso de mercaderia Con Origen desconocido por el usuario " + user.UserInfromation.FullName;
                if (actionName.Contains("Edit"))
                    action = "Se ha Editado el ingreso de mercaderia Con Origen desconocido por el usuario " + user.UserInfromation.FullName;
                if (actionName.Contains("Delete"))
                    action = "Se ha eliminado el ingreso de mercaderia Con Origen desconocido por el usuario " + user.UserInfromation.FullName;
            }
            else
            {
                if (actionName.Contains("Create"))
                    action = "hubo un ingreso de mercaderia por el usuario " + user.UserInfromation.FullName;
                if (actionName.Contains("Edit"))
                    action = "Se ha Editado el ingreso de mercaderia por el usuario " + user.UserInfromation.FullName;
                if (actionName.Contains("Delete"))
                    action = "Se ha eliminado el ingreso de mercaderia por el usuario " + user.UserInfromation.FullName;
            }
            model.Message = action;
            var listauser = _usersDao.GetAll().Where(d => d.UserRol.Description == "Admin");
            var litaUSers = new List<UserNotification>();
            foreach (var item in listauser)
            {
                var notificationuser = new UserNotification() { User = item };
                litaUSers.Add(notificationuser);
            }
            model.UserToNotifiy = litaUSers;
            _notificacionDao.Add(model);
            _notificacionDao.Save();
            return model.Id;

        }

        public override void AfterSave(Inbound model, InboundViewModel viewModel, bool isNew)
        {
            if (model.ClientOrigen.Description != null)
            {
                var desconocido = model.ClientOrigen.Description.Contains("Desconocido");
                var idNotificatio = CreateNotificationClient(model, desconocido);
                var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                Hubs.NoficationsHub j = new NoficationsHub();
                j.SendMessage(idNotificatio, id);
            }
            base.AfterSave(model, viewModel, isNew);

        }

        private int GetLastSecuence()
        {
            var model = _inboundDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Inbound model, InboundViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.Secuencia = GetLastSecuence();
                model.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateCreate = DateTime.Now;
            }
            else
            {
                model.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateUpdate = DateTime.Now;

            }
            base.BeforeSave(model, viewModel, isNew);
        }
        #region Reportes


        private MemoryStream createPDF(string html)
        {
            MemoryStream msOutput = new MemoryStream();
            TextReader reader = new StringReader(html);

            // step 1: creation of a document-object
            Document document = new Document(PageSize.A4, 30, 30, 30, 30);

            // step 2:
            // we create a writer that listens to the document
            // and directs a XML-stream to a file
            PdfWriter writer = PdfWriter.GetInstance(document, msOutput);

            // step 3: we create a worker parse the document
            HTMLWorker worker = new HTMLWorker(document);

            // step 4: we open document and start the worker on the document
            document.Open();
            worker.StartDocument();

            // step 5: parse the html into the document
            worker.Parse(reader);

            // step 6: close the document and the worker
            worker.EndDocument();
            worker.Close();
            document.Close();

            return msOutput;
        }

        public void Dosnwload(string html)
        {
            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new iTextSharp.text.Document())
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();

                        //Our sample HTML and CSS
                        var example_html = html.Replace(@"'", @"""").ToString();
                        //var sre = new StringReader(example_html);
                        //var sre = new StringReader(example_html);
                        // example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";

                        //var example_css = @".rTable {display: table;   width: 100%;} .rTableRow {display: table-row;} .rTableHeading {display: table-header-group; background-color: #ddd;} .rTableCell, .rTableHead { display: table-cell; padding: 3px 10px; border: 2px solid #999999;} .rTableHeading { display: table-header-group;  background-color: #ddd; font-weight: bold;} .rTableFoot { display: table-footer-group; font-weight: bold; background-color: #ddd;} .rTableBody { display: table-row-group;}";
                        var example_css = @"table { width: 100%;} th, td {   border: 1px solid black; width: 100%;} img {text-align: right; float: right;height:100 width:150} #Titulo { float: left; margin-top: 0px;} #divHeader{height: 200px;}";
                        example_css = example_css.Trim();
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {

                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }


                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }
            MemoryStream memoryStream = new MemoryStream(bytes);
            TextWriter textWriter = new StreamWriter(memoryStream);
            textWriter.WriteLine("Something");
            textWriter.Flush(); // added this line
            byte[] bytesInStream = bytes; // simpler way of converting to array
            memoryStream.Close();

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment;    filename=name_you_file.pdf");
            Response.BinaryWrite(bytesInStream);
            Response.End();
        }

        public iTextSharp.text.Document pagina()
        {
            return new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36);
        }

        public FileContentResult PdfList(IList<InboundViewModel> viewModels)
        {
            Byte[] bytes = new byte[1];
            ITextEvents1 eventos = new ITextEvents1();
            var ms = new MemoryStream();




            try
            {
                ms = new MemoryStream();
                try
                {
                    var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36);

                    try
                    {
                        var writer = PdfWriter.GetInstance(doc, ms); ;
                        doc.Open();
                        foreach (var viewModel in viewModels)
                        {

                            doc.NewPage();
                            writer = PdfWriter.GetInstance(doc, ms);
                            writer.PageEvent = eventos;
                            PdfPTable table2 = new PdfPTable(2);
                            table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                            InboundViewModel n = new InboundViewModel();
                            var j = n.GetType().GetProperties().Where(d => !d.Name.Contains("Id"));
                            Dictionary<string, string> _myDict = new Dictionary<string, string>();
                            string[,] matriz = new string[2, j.Count()];
                            var vector = new string[4];
                            int titulo = 0;
                            int contado = 0;
                            foreach (
                                var prop in viewModel.GetType().GetProperties().Where(d => !d.Name.Contains("Id")))
                            {
                                vector[titulo] = base.GetDisplayName(n, prop.Name);
                                if (prop.PropertyType.Name == "Boolean")
                                {
                                    var value = prop.GetValue(viewModel, null).ToString();
                                    if (value == "True")
                                    {
                                        vector[titulo + 2] = "Si";
                                    }
                                    else
                                    {
                                        vector[titulo + 2] = "No";
                                    }
                                }
                                else
                                {
                                    vector[titulo + 2] = prop.GetValue(viewModel, null).ToString();
                                }

                                if (contado == 1)
                                {
                                    PdfPCell cell1;
                                    for (int i = 0; i < vector.Length; i++)
                                    {
                                        if (i == 0 || i == 1)
                                            cell1 =
                                                new PdfPCell(new Phrase(vector[i].ToString(),
                                                    new Font(Font.NORMAL, 16, Font.BOLD)));
                                        else
                                            cell1 =
                                                new PdfPCell(new Phrase(vector[i].ToString(),
                                                    new Font(Font.NORMAL, 13, Font.NORMAL)));

                                        cell1.Colspan = 1;
                                        if ((i % 2) == 0)
                                            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        else
                                            cell1.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        table2.AddCell(cell1);

                                    }
                                    vector = new string[4];
                                    contado = 0;
                                    titulo = 0;
                                }
                                if (vector[0] != null)
                                {
                                    contado++;
                                    titulo++;
                                }
                            }
                            table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

                        }
                        doc.Close();
                    }
                    finally
                    {
                        var u = doc.PageNumber;
                        // writer.Close();
                    }
                    if (bytes.Length != 1)
                    {
                        byte[] newArray = new byte[bytes.Length + ms.ToArray().Length];
                        System.Buffer.BlockCopy(bytes, 0, newArray, 0, bytes.Length);
                        bytes = newArray;
                    }
                    else
                        bytes = ms.ToArray();

                }
                finally
                {


                }

            }
            finally
            {
                // ms.Close();

            }

            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
            return File(bytes, mimeType);
        }

        public FileContentResult Pdf(InboundViewModel viewModel)
        {
            Byte[] bytes;
            ITextEvents1 eventos = new ITextEvents1();
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        // eventos.Title = "Servicios de Logística y Distribución – Refrigerados, Congelados";
                        writer.PageEvent = eventos;

                        PdfPTable table = new PdfPTable(1);
                        //table.WidthPercentage = 100; //PdfPTable.writeselectedrows below didn't like this


                        PdfPTable table2 = new PdfPTable(2);
                        table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]
                        InboundViewModel n = new InboundViewModel();

                        var j = n.GetType().GetProperties().Where(d => !d.Name.Contains("Id"));
                        Dictionary<string, string> _myDict = new Dictionary<string, string>();
                        string[,] matriz = new string[2, j.Count()];
                        var vector = new string[4];
                        int titulo = 0;
                        int contado = 0;
                        foreach (var prop in viewModel.GetType().GetProperties().Where(d => !d.Name.Contains("Id")))
                        {


                            vector[titulo] = base.GetDisplayName(n, prop.Name);
                            if (prop.PropertyType.Name == "Boolean")
                            {
                                var value = prop.GetValue(viewModel, null).ToString();
                                if (value == "True")
                                {
                                    vector[titulo + 2] = "Si";

                                }
                                else
                                {
                                    vector[titulo + 2] = "No";
                                }
                            }
                            else
                            {
                                vector[titulo + 2] = prop.GetValue(viewModel, null) == null ? "" : prop.GetValue(viewModel, null).ToString();
                            }

                            if (contado == 1)
                            {
                                PdfPCell cell1;
                                for (int i = 0; i < vector.Length; i++)
                                {
                                    if (i == 0 || i == 1)
                                        cell1 = new PdfPCell(new Phrase(vector[i].ToString(), new Font(Font.NORMAL, 16, Font.BOLD)));
                                    else
                                        cell1 = new PdfPCell(new Phrase(vector[i].ToString(), new Font(Font.NORMAL, 13, Font.NORMAL)));

                                    cell1.Colspan = 1;
                                    if ((i % 2) == 0)
                                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    else
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;

                                    table2.AddCell(cell1);

                                }
                                vector = new string[4];
                                contado = 0;
                                titulo = 0;
                            }
                            if (vector[0] != null)
                            {
                                contado++;
                                titulo++;
                            }
                        }

                        table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
            return File(bytes, mimeType);

            // return File(System.IO.File.WriteAllBytes(testFile, bytes));
        }
        public FileContentResult Pdf2(IList<InboundViewModel> viewModels)
        {
            Byte[] bytes;
            ITextEvents1 eventos = new ITextEvents1();
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        foreach (var viewModel in viewModels)
                        {
                            doc.NewPage();
                            eventos.Titulo = "Recepcion de mercaderia";
                            writer.PageEvent = eventos;

                            var table2 = new PdfPTable(2);
                            table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]
                            InboundViewModel n = new InboundViewModel();

                            var j = n.GetType().GetProperties().Where(d => !d.Name.Contains("Id"));
                            Dictionary<string, string> _myDict = new Dictionary<string, string>();
                            string[,] matriz = new string[2, j.Count()];
                            var vector = new string[4];
                            int titulo = 0;
                            int contado = 0;
                            foreach (var prop in viewModel.GetType().GetProperties().Where(d => !d.Name.Contains("Id")))
                            {


                                vector[titulo] = base.GetDisplayName(n, prop.Name);
                                if (prop.PropertyType.Name == "Boolean")
                                {
                                    var value = prop.GetValue(viewModel, null).ToString();
                                    if (value == "True")
                                    {
                                        vector[titulo + 2] = "Si";

                                    }
                                    else
                                    {
                                        vector[titulo + 2] = "No";
                                    }
                                }
                                else
                                {
                                    vector[titulo + 2] = prop.GetValue(viewModel, null).ToString();
                                }

                                if (contado == 1)
                                {
                                    PdfPCell cell1;
                                    for (int i = 0; i < vector.Length; i++)
                                    {
                                        if (i == 0 || i == 1)
                                            cell1 = new PdfPCell(new Phrase(vector[i].ToString(), new Font(Font.NORMAL, 16, Font.BOLD)));
                                        else
                                            cell1 = new PdfPCell(new Phrase(vector[i].ToString(), new Font(Font.NORMAL, 13, Font.NORMAL)));

                                        cell1.Colspan = 1;
                                        if ((i % 2) == 0)
                                            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        else
                                            cell1.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        table2.AddCell(cell1);

                                    }
                                    vector = new string[4];
                                    contado = 0;
                                    titulo = 0;
                                }
                                if (vector[0] != null)
                                {
                                    contado++;
                                    titulo++;
                                }
                            }
                            table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

                        }

                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
            return File(bytes, mimeType);

            // return File(System.IO.File.WriteAllBytes(testFile, bytes));
        }

        public FileContentResult pfd(string html)
        {
            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;
            ITextEvents1 eventos = new ITextEvents1();
            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
                {


                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();
                        //   eventos.Title = "Servicios de Logística y Distribución – Refrigerados, Congelados";
                        writer.PageEvent = eventos;

                        //Our sample HTML and CSS
                        var example_html = html.Replace(@"'", @"""").ToString();
                        //var example_css = @".rTable {display: table;   width: 100%;} .rTableRow {display: table-row;} .rTableHeading {display: table-header-group; background-color: #ddd;} .rTableCell, .rTableHead { display: table-cell; padding: 3px 10px; border: 2px solid #999999;} .rTableHeading { display: table-header-group;  background-color: #ddd; font-weight: bold;} .rTableFoot { display: table-footer-group; font-weight: bold; background-color: #ddd;} .rTableBody { display: table-row-group;}";
                        var example_css = @".tables{border:none;} table { border-collapse: collapse; width: 100%;} th {padding: 10px; font-weight: bold; height: 50px; border: 1px solid black; width: 100%;} td {  padding: 15px; height: 20px; border: 1px solid black; width: 100%;} ";
                        example_css = example_css.Trim();

                        /**************************************************
                         * Example #1                                     *
                         *                                                *
                         * Use the built-in HTMLWorker to parse the HTML. *
                         * Only inline CSS is supported.                  *
                         * ************************************************/

                        //Create a new HTMLWorker bound to our document
                        //using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                        //{

                        //    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                        //    using (var sr = new StringReader(example_html))
                        //    {

                        //        //Parse the HTML
                        //        htmlWorker.Parse(sr);
                        //    }
                        //}

                        /**************************************************
                         * Example #2                                     *
                         *                                                *
                         * Use the XMLWorker to parse the HTML.           *
                         * Only inline CSS and absolutely linked          *
                         * CSS is supported                               *
                         * ************************************************/

                        //XMLWorker also reads from a TextReader and not directly from a string
                        //using (var srHtml = new StringReader(example_html))
                        //{

                        //    //Parse the HTML
                        //    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        //}

                        /**************************************************
                         * Example #3                                     *
                         *                                                *
                         * Use the XMLWorker to parse HTML and CSS        *
                         * ************************************************/

                        //In order to read CSS as a string we need to switch to a different constructor
                        //that takes Streams instead of TextReaders.
                        //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {

                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }


                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }

            //Now we just need to do something with those bytes.
            //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
            //You could also write the bytes to a database in a varbinary() column (but please don't) or you
            //could pass them to another function for further PDF processing.
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
            return File(bytes, mimeType);

            // return File(System.IO.File.WriteAllBytes(testFile, bytes));
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public FileContentResult Print(int id)
        {

            var model = _iViewmodelMapper.Map(_inboundDao.Get(id));
            //var view = RenderRazorViewToString("ReportPrint", model);

            // PartialViewAsPdf("_PartialViewTest", model){FileName = "TestPartialViewAsPdf.pdf" };
            return Pdf(model);
        }
        public FileContentResult Print2()
        {

            var model = _iViewmodelMapper.Map(_inboundDao.GetAll()).ToList();
            //var modelview = RenderRazorViewToString("ReportPrint", model);

            // PartialViewAsPdf("_PartialViewTest", model){FileName = "TestPartialViewAsPdf.pdf" };
            return Pdf2(model);
        }
        public ActionResult Report(int id)
        {
            var model = _iViewmodelMapper.Map(_inboundDao.Get(id));
            return PartialView(model);
        }
        public ActionResult ReportPrint(int id)
        {
            var model = _iViewmodelMapper.Map(_inboundDao.Get(id));
            return PartialView(model);
        }
        #endregion
        [HttpPost]
        public override ActionResult Create(InboundViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    var module =
                        user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(
                            v => v.Module.Name == "RecepcionMercaderia");
                    if (module != null)
                    {
                        ViewBag.Add = module.Actions.Any(s => s.Description == "AgregarCuenta");
                    }
                    else
                    {
                        ViewBag.Add = false;

                    }
                }
                ViewBag.Users = _usersVmm.Map(_usersDao.GetAll());
                ViewBag.Clients = _clientsDao.GetAllClients();
                ViewBag.ClientsDestino = _clientsDao.GetAllDestinoList();
                ViewBag.ClientsOrigen = _clientsDao.GetAllOrigenList();
                ViewBag.Measures = _measureDao.GetAll();
                ViewBag.Package = _packageDao.GetAll();
                ViewBag.Products = _productsDao.GetAll();
                ViewBag.Bills = _billDao.GetAll();
                ViewBag.BillsType = _billTypeDao.GetAll();
                return PartialView(viewModel);
            }

            var mode = base.CreateModel(viewModel);
            var mensaje = "se ha creado la orden Numero: " + mode.Secuencia.ToString();
            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModalShowData(mensaje));
        }

        public override ActionResult Edit(InboundViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = _usersVmm.Map(_usersDao.GetAll());
                ViewBag.ClientsOrigen = _clientsDao.GetAllOrigenList();
                ViewBag.Clients = _clientsDao.GetAllClients();
                ViewBag.ClientsDestino = _clientsDao.GetAllDestinoList();
                ViewBag.Measures = _measureDao.GetAll();
                ViewBag.Package = _packageDao.GetAll();
                ViewBag.Products = _productsDao.GetAll();
                ViewBag.Bills = _billDao.GetAll();
                ViewBag.BillsType = _billTypeDao.GetAll();
                if (User.Identity.IsAuthenticated)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "RecepcionMercaderia");
                    if (module != null)
                    {
                        ViewBag.Add = module.Actions.Any(s => s.Description == "AgregarCuenta");
                    }
                    else
                    {
                        ViewBag.Add = false;

                    }
                }
                return PartialView(viewModel);
            }

            return base.Edit(viewModel);
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.Users = _usersVmm.Map(_usersDao.GetAll());
            ViewBag.ClientsOrigen = _clientsDao.GetAllOrigenList();
            ViewBag.Clients = _clientsDao.GetAllClients();
            ViewBag.ClientsDestino = _clientsDao.GetAllDestinoList();
            ViewBag.Measures = _measureDao.GetAll();
            ViewBag.Package = _packageDao.GetAll();
            ViewBag.Products = _productsDao.GetAll();
            ViewBag.Bills = _billDao.GetAll();
            ViewBag.BillsType = _billTypeDao.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "RecepcionMercaderia");
                if (module != null)
                {
                    ViewBag.Add = module.Actions.Any(s => s.Description == "AgregarCuenta");
                }
                else
                {
                    ViewBag.Add = false;

                }
            }
            return base.Edit(id);
        }

        public JsonResult CheckUbications(string text)
        {
            var ids = text.Split('-');
            var idubication = new List<int>();
            var resultado = true;
            var strResult = "Los siguiente destinos: ";
            for (int i = 0; i < ids.Count(); i++)
            {
                if (ids[i] != "" && !idubication.Contains(Convert.ToInt32(ids[i])))
                {
                    var mode = _inboundDao.Get(Convert.ToInt32(ids[i]));
                    if (mode.ClientTo.Ubication == null)
                    {
                        strResult = strResult + " " + mode.ClientTo.Alias;
                        resultado = false;
                    }
                }

            }
            strResult = strResult + "  no Tienen Zona Asignada";
            return Json(
                            (new
                            {
                               resultado=resultado,
                               cuentas=strResult

                            }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult KilosTotales(string text)
        {
            var ids = text.Split('-');
            var idubication = new List<int>();
            var resultado = true;
            decimal strResult = 0;
            int contador = 0;
            decimal usoFisico = 0;
            for (int i = 0; i < ids.Count(); i++)
            {
                if (ids[i] != "" )
                {
                    var mode = _inboundDao.Get(Convert.ToInt32(ids[i]));
                    if (mode != null)
                    {
                        strResult = strResult + Convert.ToDecimal(mode.Kilos);
                        usoFisico = usoFisico + mode.UsoFisico;
                        contador++;
                    }
                }

            }
            
            return Json(
                            (new
                            {
                                resultado = strResult,
                                usoFisico=usoFisico,
                                contador=contador
                              }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductsPrice(string text)
        {
            var cargas = _inboundDao.Get(Convert.ToInt32(text));
            if (cargas != null)
            {
                return Json(
                             (new
                             {
                                 id = cargas.Id,
                                 price = cargas.PriceDecimal,
                                 quantity = cargas.Quantity,
                                 kilos = cargas.Kilos,
                                 description = cargas.Description,

                             }), JsonRequestBehavior.AllowGet);
            }


            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        private IList<SelectListItem> GetItems()
        {
            var list = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Selecione una Camion" } };
            var lista = _camionesDao.GetAll().Where(r => !r.IsTraveling || !r.IsMaintenace && !r.IsDelete);
            foreach (var item in lista)
            {
                var items = new SelectListItem() { Value = item.Id.ToString(), Text = item.Nombre };
                list.Add(items);

            }
            return list;
        }
        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.camiones = GetItems();
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "RecepcionMercaderia");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.ver = module.Actions.Any(s => s.Description == "Ver");
                    ViewBag.crearViaje = module.Actions.Any(s => s.Description == "Crear Viaje");
                    ViewBag.cargar = module.Actions.Any(s => s.Description == "Cargar");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.ver = false;
                    ViewBag.crearViaje = false;
                    ViewBag.cargar = false;
                }
            }
            ViewBag.Title = "Recepcion de Mercaderia";
            return PartialView();
        }
        public ActionResult IndexRestore()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "RecepcionMercaderia");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                }
            }
            ViewBag.Title = "Recepcion de Mercaderia";
            return PartialView();
        }
    }
}