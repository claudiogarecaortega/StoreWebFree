using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.util;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BassinoBase.Models.ViewModelMapper.Interface;
using BassinoLibrary.Resource;
using BassinoLibrary.ViewModels;
using Domain.Commodity;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public class ReportController : AbmController<Inbound, InboundViewModel, InboundViewModel>
    {
        #region Privados
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IUserdDao _userDao;
        private readonly IUserViewModelMapper _userViewModelMapper;
        private readonly IRolesDao _rolesDao;

        private readonly IUserdDao _usersDao;
        private readonly IClientDao _clientsDao;
        private readonly IMeasureUnitDao _measureDao;
        private readonly IPackageTypeDao _packageDao;
        private readonly IProductDao _productsDao;
        private readonly IBillDao _billDao;
        private readonly IBillTypeDao _billTypeDao;
        private readonly IInboundDao _inboundDao;
        private readonly IInboundViewModelMapper _iViewmodelMapper;
        private readonly IProductViewModelMapper _productViewModelMapper;
        private readonly IClientViewModelMapper _iclientViewModelMapper;
        private readonly IProviderViewModelMapper _iproviderViewModelMapper;
        private readonly IProviderDao _providerDao;

        public ReportController(ApplicationSignInManager _SignInManager, ApplicationUserManager _UserManager, IInboundViewModelMapper inboundViewModelMapper, IInboundDao inboundDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IUserdDao usersDao, IClientDao clientsDao, IMeasureUnitDao measureDao, IPackageTypeDao packageDao, IProductDao productsDao, IBillDao billDao, IBillTypeDao billTypeDao, IInboundDao inboundDao1, IInboundViewModelMapper iViewmodelMapper, IProductViewModelMapper productViewModelMapper, IClientViewModelMapper iclientViewModelMapper, IProviderViewModelMapper iproviderViewModelMapper, IProviderDao providerDao)
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
            _productViewModelMapper = productViewModelMapper;
            _iclientViewModelMapper = iclientViewModelMapper;
            _iproviderViewModelMapper = iproviderViewModelMapper;
            _providerDao = providerDao;
            _signInManager = _SignInManager;// HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = _UserManager;// HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        }


        #endregion
        #region Exportacion a Excel
        //exportar productos
        public ActionResult PrintAllProductsExcel()
        {
            var products = new System.Data.DataTable("teste");
            products.Columns.Add("Numero de Producto", typeof(int));
            products.Columns.Add("Descripcion", typeof(string));
            products.Columns.Add("Necesita Frio", typeof(string));
            var productosActivos = _productsDao.GetAll().Where(d => !d.IsDelete);
            foreach (var item in productosActivos)
            {
                products.Rows.Add(item.Id, item.Description, item.IsCold ? "Si" : "No");
            }
            return PrintAllExcel(products);


        }
        //Exportar todos los Clientes
        public ActionResult PrintAllClientsExcel()
        {
            var products = new System.Data.DataTable("teste");
            products.Columns.Add("Numero de Producto", typeof(int));
            products.Columns.Add("Descripcion", typeof(string));
            products.Columns.Add("Necesita Frio", typeof(string));
            var productosActivos = _productsDao.GetAll().Where(d => !d.IsDelete);
            foreach (var item in productosActivos)
            {
                products.Rows.Add(item.Id, item.Description, item.IsCold ? "Si" : "No");
            }
            return PrintAllExcel(products);


        }
        public ActionResult PrintAllExcel(DataTable datos)
        {

            var grid = new GridView();
            grid.DataSource = datos;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            var tableHead = "";
            var pathImage=""+ System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/logoazul.jpg") + "";
            tableHead =
                "<table style='width:100%'><tr ><td style='width:30%' align='left'> Bassino</td><td ></td> <td align='right' rowspan=2> <img src='"+pathImage+"' height=100 width=200/></td></tr><tr><td></td><td align='center'>Reporte de todos los :</td> </tr><tr><td align='justify'>Empresa de logistica y transporte</td><td align='center'>Productos</td> <td align='right'>" + DateTime.Now.Date.ToString() + "</td></tr></table>";
            StringWriter sw = new StringWriter();
            sw.Write(tableHead);

            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.AddAttribute(HtmlTextWriterAttribute.Src, pathImage);
            htw.AddAttribute(HtmlTextWriterAttribute.Width, "60");
            htw.AddAttribute(HtmlTextWriterAttribute.Height, "60");
            htw.AddAttribute(HtmlTextWriterAttribute.Alt, "");

            grid.RenderControl(htw);

            Response.Write(tableHead);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModal(MensajeOperacionExitosa));


        }

        #endregion
        #region Propiedades


        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }


        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }
        #endregion
        public ActionResult Index()
        {
            return PartialView();
        }
        #region Productos
        public FileContentResult PrintProductById(int id)
        {
            var model = _productViewModelMapper.Map(_productsDao.Get(id));
            var lsit = new List<ProductViewModel>();
            lsit.Add(model);
            return AllProducts(lsit);

        }
        public FileContentResult PrintAllProducts()
        {
            var model = _productViewModelMapper.Map(_productsDao.GetAll()).ToList();
            return AllProducts(model);

        }
        public FileContentResult AllProducts(IList<ProductViewModel> viewModels)
        {

            Byte[] bytes;
            ITextEvents1 eventos = new ITextEvents1();
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        int i = 0;
                        int j = 0;
                        int cantidad = Convert.ToInt32(Math.Round((doc.PageSize.Height - 260) / 80));
                        doc.Open();
                        foreach (var viewModel in viewModels)
                        {

                            if (i == cantidad || j == viewModels.Count)
                            {
                                doc.NewPage();
                                i = 0;
                            }
                            eventos.Titulo = "Productos";
                            writer.PageEvent = eventos;

                            var table2 = new PdfPTable(2);
                            table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]
                            ProductViewModel n = new ProductViewModel();

                            PdfPCell cell0;
                            cell0 = new PdfPCell(new Phrase("Producto n°:  " + viewModel.Id, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell0.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell0.Colspan = 2;

                            PdfPCell cell1;
                            cell1 = new PdfPCell(new Phrase("Descripcion", new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell1.Colspan = 1;

                            PdfPCell cell2;
                            cell2 = new PdfPCell(new Phrase(viewModel.Description, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell2.Colspan = 1;

                            PdfPCell cell3;
                            cell3 = new PdfPCell(new Phrase("Necesita Frio?", new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell3.Colspan = 1;

                            PdfPCell cell4;
                            if (viewModel.IsCold)
                            {
                                cell4 = new PdfPCell(new Phrase("Si", new Font(Font.NORMAL, 13, Font.NORMAL)));
                            }
                            else
                            {
                                cell4 = new PdfPCell(new Phrase("No", new Font(Font.NORMAL, 13, Font.NORMAL)));
                            }

                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell4.Colspan = 1;

                            table2.AddCell(cell0);
                            table2.AddCell(cell1);
                            table2.AddCell(cell3);
                            table2.AddCell(cell2);
                            table2.AddCell(cell4);
                            i++;
                            j++;
                            if (i == 1)
                                table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);
                            else
                                table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - (130 + ((i - 1) * (table2.TotalHeight + 30))), writer.DirectContent);

                        }

                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
            return File(bytes, mimeType);
        }
        #endregion
        #region Cuenta

        //public FileContentResult PrintAllClients()
        //{
        //    var models = _iclientViewModelMapper.Map(_clientsDao.GetAllClients()).ToList();
        //    return AllCuentas(models);
        //}
        //public FileContentResult PrintClientById(int id)
        //{
        //    var models = _iclientViewModelMapper.Map(_clientsDao.Get(id));
        //    var list = new List<ClientViewModel>();
        //    list.Add(models);
        //    return AllCuentas(list);
        //}

        //public FileContentResult AllCuentas(IList<ClientViewModel> viewModels)
        //{

        //    Byte[] bytes;
        //    ITextEvents1 eventos = new ITextEvents1();
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
        //        {
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {
        //                doc.Open();
        //                foreach (var viewModel in viewModels)
        //                {

        //                    doc.NewPage();

        //                    eventos.Titulo = "Cuentas";
        //                    writer.PageEvent = eventos;

        //                    var table2 = new PdfPTable(2);
        //                    table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]
        //                    ProductViewModel n = new ProductViewModel();

        //                    PdfPCell cell0;
        //                    cell0 = new PdfPCell(new Phrase("Cuenta n°:  " + viewModel.Id, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell0.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cell0.Colspan = 2;

        //                    PdfPCell cell1;
        //                    cell1 = new PdfPCell(new Phrase(Resources.NameOrSocial, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell1.Colspan = 1;
        //                    PdfPCell cell2;
        //                    cell2 = new PdfPCell(new Phrase(viewModel.Name, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell2.Colspan = 1;

        //                    PdfPCell cell3;
        //                    cell3 = new PdfPCell(new Phrase(Resources.Direction, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell3.Colspan = 1;
        //                    PdfPCell cell4;
        //                    cell4 = new PdfPCell(new Phrase(viewModel.Direction, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell4.Colspan = 1;

        //                    PdfPCell cell5;
        //                    cell5 = new PdfPCell(new Phrase(Resources.TaxConditionLabel, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell5.Colspan = 1;
        //                    PdfPCell cell6;
        //                    cell6 = new PdfPCell(new Phrase(viewModel.TaxCondition, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell6.Colspan = 1;


        //                    PdfPCell cell7;
        //                    cell7 = new PdfPCell(new Phrase(Resources.Cuit, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell7.Colspan = 1;
        //                    PdfPCell cell8;
        //                    cell8 = new PdfPCell(new Phrase(viewModel.Cuit, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell8.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell8.Colspan = 1;

        //                    PdfPCell cell9;
        //                    cell9 = new PdfPCell(new Phrase(Resources.IiBb, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell9.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell9.Colspan = 1;
        //                    PdfPCell cell10;
        //                    cell10 = new PdfPCell(new Phrase(viewModel.IiBb, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell10.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell10.Colspan = 1;


        //                    PdfPCell cell11;
        //                    cell11 = new PdfPCell(new Phrase(Resources.NameContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell11.Colspan = 1;
        //                    PdfPCell cell12;
        //                    cell12 = new PdfPCell(new Phrase(viewModel.NameContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell12.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell12.Colspan = 1;


        //                    PdfPCell cell13;
        //                    cell13 = new PdfPCell(new Phrase(Resources.PhoneContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell13.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell13.Colspan = 1;
        //                    PdfPCell cell14;
        //                    cell14 = new PdfPCell(new Phrase(viewModel.PhoneContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell14.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell14.Colspan = 1;


        //                    PdfPCell cell15;
        //                    cell15 = new PdfPCell(new Phrase(Resources.MailContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell15.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell15.Colspan = 1;
        //                    PdfPCell cell16;
        //                    cell16 = new PdfPCell(new Phrase(viewModel.MailContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell16.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell16.Colspan = 1;


        //                    PdfPCell cell17;
        //                    cell17 = new PdfPCell(new Phrase(Resources.Services, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell17.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell17.Colspan = 1;
        //                    PdfPCell cell18;
        //                    cell18 = new PdfPCell(new Phrase(viewModel.ServicesesDescription, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell18.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell18.Colspan = 1;


        //                    PdfPCell cell19;
        //                    cell19 = new PdfPCell(new Phrase(Resources.ServicePrice, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell19.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell19.Colspan = 1;
        //                    PdfPCell cell20;
        //                    cell20 = new PdfPCell(new Phrase(viewModel.ServicePrice.ToString(), new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell20.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell20.Colspan = 1;


        //                    PdfPCell cell21;
        //                    cell21 = new PdfPCell(new Phrase(Resources.MeasureUnitClient, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell21.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell21.Colspan = 1;
        //                    PdfPCell cell22;
        //                    cell22 = new PdfPCell(new Phrase(viewModel.MeasureUnit, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell22.Colspan = 1;


        //                    PdfPCell cell23;
        //                    cell23 = new PdfPCell(new Phrase(Resources.Comentaries, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell23.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell23.Colspan = 2;
        //                    PdfPCell cell24;
        //                    cell24 = new PdfPCell(new Phrase(viewModel.Description, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell24.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell24.Colspan = 2;
        //                    cell24.Rowspan = 2;

        //                    table2.AddCell(cell0);
        //                    table2.AddCell(cell1);
        //                    table2.AddCell(cell3);
        //                    table2.AddCell(cell2);
        //                    table2.AddCell(cell4);
        //                    table2.AddCell(cell5);
        //                    table2.AddCell(cell7);
        //                    table2.AddCell(cell6);
        //                    table2.AddCell(cell8);
        //                    table2.AddCell(cell9);
        //                    table2.AddCell(cell11);
        //                    table2.AddCell(cell10);
        //                    table2.AddCell(cell12);
        //                    table2.AddCell(cell13);
        //                    table2.AddCell(cell15);
        //                    table2.AddCell(cell14);
        //                    table2.AddCell(cell16);
        //                    table2.AddCell(cell17);
        //                    table2.AddCell(cell19);
        //                    table2.AddCell(cell18);
        //                    table2.AddCell(cell20);
        //                    table2.AddCell(cell21);
        //                    table2.AddCell(cell22);
        //                    table2.AddCell(cell23);
        //                    table2.AddCell(cell24);

        //                    table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

        //                }

        //                doc.Close();
        //            }
        //        }
        //        bytes = ms.ToArray();
        //    }
        //    string mimeType = "application/pdf";
        //    Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
        //    return File(bytes, mimeType);
        //}
        #endregion
        #region Origen
        //public FileContentResult PrintAllClientsOrigen()
        //{
        //    var models = _iclientViewModelMapper.Map(_clientsDao.GetAllOrigenList()).ToList();
        //    return AllCuentasOrigen(models);
        //}
        //public FileContentResult PrintClientOrgigenById(int id)
        //{
        //    var models = _iclientViewModelMapper.Map(_clientsDao.Get(id));
        //    var list = new List<ClientViewModel>();
        //    list.Add(models);
        //    return AllCuentasOrigen(list);
        //}

        //public FileContentResult AllCuentasOrigen(IList<ClientViewModel> viewModels)
        //{

        //    Byte[] bytes;
        //    ITextEvents1 eventos = new ITextEvents1();
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
        //        {
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {
        //                doc.Open();
        //                foreach (var viewModel in viewModels)
        //                {

        //                    doc.NewPage();

        //                    eventos.Titulo = "Origenes ";
        //                    writer.PageEvent = eventos;

        //                    var table2 = new PdfPTable(2);
        //                    table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]
        //                    ProductViewModel n = new ProductViewModel();

        //                    PdfPCell cell0;
        //                    cell0 = new PdfPCell(new Phrase("Origen n°:  " + viewModel.Id, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell0.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cell0.Colspan = 2;

        //                    PdfPCell cell1;
        //                    cell1 = new PdfPCell(new Phrase(Resources.NameOrSocial, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell1.Colspan = 1;
        //                    PdfPCell cell2;
        //                    cell2 = new PdfPCell(new Phrase(viewModel.Name, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell2.Colspan = 1;

        //                    PdfPCell cell3;
        //                    cell3 = new PdfPCell(new Phrase(Resources.Direction, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell3.Colspan = 1;
        //                    PdfPCell cell4;
        //                    cell4 = new PdfPCell(new Phrase(viewModel.Direction, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell4.Colspan = 1;

        //                    PdfPCell cell5;
        //                    cell5 = new PdfPCell(new Phrase(Resources.TaxConditionLabel, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell5.Colspan = 1;
        //                    PdfPCell cell6;
        //                    cell6 = new PdfPCell(new Phrase(viewModel.TaxCondition, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell6.Colspan = 1;


        //                    PdfPCell cell7;
        //                    cell7 = new PdfPCell(new Phrase(Resources.Cuit, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell7.Colspan = 1;
        //                    PdfPCell cell8;
        //                    cell8 = new PdfPCell(new Phrase(viewModel.Cuit, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell8.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell8.Colspan = 1;

        //                    PdfPCell cell9;
        //                    cell9 = new PdfPCell(new Phrase(Resources.IiBb, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell9.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell9.Colspan = 1;
        //                    PdfPCell cell10;
        //                    cell10 = new PdfPCell(new Phrase(viewModel.IiBb, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell10.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell10.Colspan = 1;


        //                    PdfPCell cell11;
        //                    cell11 = new PdfPCell(new Phrase(Resources.NameContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell11.Colspan = 1;
        //                    PdfPCell cell12;
        //                    cell12 = new PdfPCell(new Phrase(viewModel.NameContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell12.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell12.Colspan = 1;


        //                    PdfPCell cell13;
        //                    cell13 = new PdfPCell(new Phrase(Resources.PhoneContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell13.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell13.Colspan = 1;
        //                    PdfPCell cell14;
        //                    cell14 = new PdfPCell(new Phrase(viewModel.PhoneContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell14.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell14.Colspan = 1;


        //                    PdfPCell cell15;
        //                    cell15 = new PdfPCell(new Phrase(Resources.MailContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell15.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell15.Colspan = 1;
        //                    PdfPCell cell16;
        //                    cell16 = new PdfPCell(new Phrase(viewModel.MailContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell16.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell16.Colspan = 1;


        //                    PdfPCell cell23;
        //                    cell23 = new PdfPCell(new Phrase(Resources.Comentaries, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell23.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell23.Colspan = 2;
        //                    PdfPCell cell24;
        //                    cell24 = new PdfPCell(new Phrase(viewModel.Description, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell24.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell24.Colspan = 2;
        //                    cell24.Rowspan = 2;

        //                    table2.AddCell(cell0);
        //                    table2.AddCell(cell1);
        //                    table2.AddCell(cell3);
        //                    table2.AddCell(cell2);
        //                    table2.AddCell(cell4);
        //                    table2.AddCell(cell5);
        //                    table2.AddCell(cell7);
        //                    table2.AddCell(cell6);
        //                    table2.AddCell(cell8);
        //                    table2.AddCell(cell9);
        //                    table2.AddCell(cell11);
        //                    table2.AddCell(cell10);
        //                    table2.AddCell(cell12);
        //                    table2.AddCell(cell13);
        //                    table2.AddCell(cell15);
        //                    table2.AddCell(cell14);
        //                    table2.AddCell(cell16);
        //                    table2.AddCell(cell23);
        //                    table2.AddCell(cell24);

        //                    table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

        //                }

        //                doc.Close();
        //            }
        //        }
        //        bytes = ms.ToArray();
        //    }
        //    string mimeType = "application/pdf";
        //    Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
        //    return File(bytes, mimeType);
        //}
        //
        #endregion
        #region Destino
        //public FileContentResult PrintAllClientsDestino()
        //{
        //    var models = _iclientViewModelMapper.Map(_clientsDao.GetAllDestinoList()).ToList();
        //    return AllCuentasDestino(models);
        //}
        //public FileContentResult PrintClientByIdDestino(int id)
        //{
        //    var models = _iclientViewModelMapper.Map(_clientsDao.Get(id));
        //    var list = new List<ClientViewModel>();
        //    list.Add(models);
        //    return AllCuentasDestino(list);
        //}

        //public FileContentResult AllCuentasDestino(IList<ClientViewModel> viewModels)
        //{

        //    Byte[] bytes;
        //    ITextEvents1 eventos = new ITextEvents1();
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var doc = new iTextSharp.text.Document(PageSize.A4, 36, 36, 36, 36))
        //        {
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {
        //                doc.Open();
        //                foreach (var viewModel in viewModels)
        //                {

        //                    doc.NewPage();

        //                    eventos.Titulo = "Cuentas";
        //                    writer.PageEvent = eventos;

        //                    var table2 = new PdfPTable(2);
        //                    table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]
        //                    ProductViewModel n = new ProductViewModel();

        //                    PdfPCell cell0;
        //                    cell0 = new PdfPCell(new Phrase("Cuenta n°:  " + viewModel.Id, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell0.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    cell0.Colspan = 2;

        //                    PdfPCell cell1;
        //                    cell1 = new PdfPCell(new Phrase(Resources.NameOrSocial, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell1.Colspan = 1;
        //                    PdfPCell cell2;
        //                    cell2 = new PdfPCell(new Phrase(viewModel.Name, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell2.Colspan = 1;

        //                    PdfPCell cell3;
        //                    cell3 = new PdfPCell(new Phrase(Resources.DeliveryDirection, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell3.Colspan = 1;
        //                    PdfPCell cell4;
        //                    cell4 = new PdfPCell(new Phrase(viewModel.DeliveryDirection, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell4.Colspan = 1;

        //                    PdfPCell cell5;
        //                    cell5 = new PdfPCell(new Phrase(Resources.Neighborhood, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell5.Colspan = 1;
        //                    PdfPCell cell6;
        //                    cell6 = new PdfPCell(new Phrase(viewModel.Neighborhood, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell6.Colspan = 1;

        //                    PdfPCell cell5_1;
        //                    cell5_1 = new PdfPCell(new Phrase(Resources.Localidad, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5_1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell5_1.Colspan = 1;
        //                    PdfPCell cell6_1;
        //                    cell6_1 = new PdfPCell(new Phrase(viewModel.Localidad, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6_1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell6_1.Colspan = 1;


        //                    PdfPCell cell5_2;
        //                    cell5_2 = new PdfPCell(new Phrase(Resources.TimeToDelivery, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5_2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell5_2.Colspan = 1;
        //                    PdfPCell cell6_2;
        //                    cell6_2 = new PdfPCell(new Phrase(viewModel.TimeToDelivery, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6_2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell6_2.Colspan = 1;


        //                    PdfPCell cell5_3;
        //                    cell5_3 = new PdfPCell(new Phrase(Resources.Ubication, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5_3.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell5_3.Colspan = 1;
        //                    PdfPCell cell6_3;
        //                    cell6_3 = new PdfPCell(new Phrase(viewModel.Ubication, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6_3.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell6_3.Colspan = 1;


        //                    PdfPCell cell5_4;
        //                    cell5_4 = new PdfPCell(new Phrase(Resources.Sequence, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5_4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell5_4.Colspan = 1;
        //                    PdfPCell cell6_4;
        //                    cell6_4 = new PdfPCell(new Phrase(viewModel.Sequence.ToString(), new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6_4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell6_4.Colspan = 1;


        //                    PdfPCell cell5_5;
        //                    cell5_5 = new PdfPCell(new Phrase(Resources.TaxConditionLabel, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell5_5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell5_5.Colspan = 1;
        //                    PdfPCell cell6_5;
        //                    cell6_5 = new PdfPCell(new Phrase(viewModel.TaxCondition, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell6_5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell6_5.Colspan = 1;

        //                    PdfPCell cell7;
        //                    cell7 = new PdfPCell(new Phrase(Resources.Cuit, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell7.Colspan = 1;
        //                    PdfPCell cell8;
        //                    cell8 = new PdfPCell(new Phrase(viewModel.Cuit, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell8.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell8.Colspan = 1;

        //                    PdfPCell cell9;
        //                    cell9 = new PdfPCell(new Phrase(Resources.IiBb, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell9.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell9.Colspan = 1;
        //                    PdfPCell cell10;
        //                    cell10 = new PdfPCell(new Phrase(viewModel.IiBb, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell10.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell10.Colspan = 1;


        //                    PdfPCell cell11;
        //                    cell11 = new PdfPCell(new Phrase(Resources.NameContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell11.Colspan = 1;
        //                    PdfPCell cell12;
        //                    cell12 = new PdfPCell(new Phrase(viewModel.NameContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell12.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell12.Colspan = 1;


        //                    PdfPCell cell13;
        //                    cell13 = new PdfPCell(new Phrase(Resources.PhoneContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell13.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell13.Colspan = 1;
        //                    PdfPCell cell14;
        //                    cell14 = new PdfPCell(new Phrase(viewModel.PhoneContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell14.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell14.Colspan = 1;


        //                    PdfPCell cell15;
        //                    cell15 = new PdfPCell(new Phrase(Resources.MailContact, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell15.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell15.Colspan = 1;
        //                    PdfPCell cell16;
        //                    cell16 = new PdfPCell(new Phrase(viewModel.MailContact, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell16.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    cell16.Colspan = 1;



        //                    PdfPCell cell23;
        //                    cell23 = new PdfPCell(new Phrase(Resources.Comentaries, new Font(Font.NORMAL, 16, Font.BOLD)));
        //                    cell23.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell23.Colspan = 2;
        //                    PdfPCell cell24;
        //                    cell24 = new PdfPCell(new Phrase(viewModel.Description, new Font(Font.NORMAL, 13, Font.NORMAL)));
        //                    cell24.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cell24.Colspan = 2;
        //                    cell24.Rowspan = 2;

        //                    table2.AddCell(cell0);
        //                    table2.AddCell(cell1);
        //                    table2.AddCell(cell3);
        //                    table2.AddCell(cell2);
        //                    table2.AddCell(cell4);
        //                    table2.AddCell(cell5);
        //                    table2.AddCell(cell7);
        //                    table2.AddCell(cell6);
        //                    table2.AddCell(cell8);

        //                    table2.AddCell(cell5_1);
        //                    table2.AddCell(cell5_2);
        //                    table2.AddCell(cell6_1);
        //                    table2.AddCell(cell6_2);


        //                    table2.AddCell(cell5_3);
        //                    table2.AddCell(cell5_4);
        //                    table2.AddCell(cell6_3);
        //                    table2.AddCell(cell6_4);


        //                    table2.AddCell(cell5_5);
        //                    table2.AddCell(cell6_5);


        //                    table2.AddCell(cell9);
        //                    table2.AddCell(cell11);
        //                    table2.AddCell(cell10);
        //                    table2.AddCell(cell12);
        //                    table2.AddCell(cell13);
        //                    table2.AddCell(cell15);
        //                    table2.AddCell(cell14);
        //                    table2.AddCell(cell16);
        //                    table2.AddCell(cell23);
        //                    table2.AddCell(cell24);

        //                    table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

        //                }

        //                doc.Close();
        //            }
        //        }
        //        bytes = ms.ToArray();
        //    }
        //    string mimeType = "application/pdf";
        //    Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
        //    return File(bytes, mimeType);
        //}

        #endregion
        #region Proveedor
        public FileContentResult PrintAllPproviders()
        {
            var models = _iproviderViewModelMapper.Map(_providerDao.GetAll()).ToList();
            return AllPproviders(models);
        }
        public FileContentResult PrintProviderById(int id)
        {
            var models = _iproviderViewModelMapper.Map(_providerDao.Get(id));
            var list = new List<ProviderViewModel>();
            list.Add(models);
            return AllPproviders(list);
        }

        public FileContentResult AllPproviders(IList<ProviderViewModel> viewModels)
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

                            eventos.Titulo = "Provedores";
                            writer.PageEvent = eventos;

                            var table2 = new PdfPTable(2);
                            table2.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin; //this centers [table]

                            PdfPCell cell0;
                            cell0 = new PdfPCell(new Phrase("Provedor n°:  " + viewModel.Id, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell0.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell0.Colspan = 2;

                            PdfPCell cell1;
                            cell1 = new PdfPCell(new Phrase(Resources.NameOrSocial, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell1.Colspan = 1;
                            PdfPCell cell2;
                            cell2 = new PdfPCell(new Phrase(viewModel.Name, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell2.Colspan = 1;

                            PdfPCell cell3;
                            cell3 = new PdfPCell(new Phrase(Resources.Direction, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell3.Colspan = 1;
                            PdfPCell cell4;
                            cell4 = new PdfPCell(new Phrase(viewModel.Direction, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell4.Colspan = 1;

                            PdfPCell cell5;
                            cell5 = new PdfPCell(new Phrase(Resources.Ubication, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell5.Colspan = 1;
                            PdfPCell cell6;
                            cell6 = new PdfPCell(new Phrase(viewModel.Ubication, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell6.Colspan = 1;

                            PdfPCell cell7;
                            cell7 = new PdfPCell(new Phrase(Resources.ZipCode, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell7.Colspan = 1;
                            PdfPCell cell8;
                            cell8 = new PdfPCell(new Phrase(viewModel.ZipCode.ToString(), new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell8.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell8.Colspan = 1;

                            PdfPCell cell9;
                            cell9 = new PdfPCell(new Phrase(Resources.NameContact, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell9.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell9.Colspan = 1;
                            PdfPCell cell10;
                            cell10 = new PdfPCell(new Phrase(viewModel.Contact, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell10.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell10.Colspan = 1;


                            PdfPCell cell11;
                            cell11 = new PdfPCell(new Phrase(Resources.TaxCondition, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell11.Colspan = 1;
                            PdfPCell cell12;
                            cell12 = new PdfPCell(new Phrase(viewModel.TaxCondition, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell12.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell12.Colspan = 1;


                            PdfPCell cell13;
                            cell13 = new PdfPCell(new Phrase(Resources.IiBb, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell13.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell13.Colspan = 1;
                            PdfPCell cell14;
                            cell14 = new PdfPCell(new Phrase(viewModel.IiBb.ToString(), new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell14.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell14.Colspan = 1;


                            PdfPCell cell15;
                            cell15 = new PdfPCell(new Phrase(Resources.Cuit, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell15.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell15.Colspan = 1;
                            PdfPCell cell16;
                            cell16 = new PdfPCell(new Phrase(viewModel.Cuit, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell16.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell16.Colspan = 1;



                            PdfPCell cell23;
                            cell23 = new PdfPCell(new Phrase(Resources.Comentaries, new Font(Font.NORMAL, 16, Font.BOLD)));
                            cell23.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell23.Colspan = 2;
                            PdfPCell cell24;
                            cell24 = new PdfPCell(new Phrase(viewModel.Description, new Font(Font.NORMAL, 13, Font.NORMAL)));
                            cell24.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell24.Colspan = 2;
                            cell24.Rowspan = 2;

                            table2.AddCell(cell0);
                            table2.AddCell(cell1);
                            table2.AddCell(cell3);
                            table2.AddCell(cell2);
                            table2.AddCell(cell4);
                            table2.AddCell(cell5);
                            table2.AddCell(cell7);
                            table2.AddCell(cell6);
                            table2.AddCell(cell8);
                            table2.AddCell(cell9);
                            table2.AddCell(cell11);
                            table2.AddCell(cell10);
                            table2.AddCell(cell12);
                            table2.AddCell(cell13);
                            table2.AddCell(cell15);
                            table2.AddCell(cell14);
                            table2.AddCell(cell16);
                            table2.AddCell(cell23);
                            table2.AddCell(cell24);

                            table2.WriteSelectedRows(0, -1, doc.Left, doc.Top - 130, writer.DirectContent);

                        }

                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + "test.pdf");
            return File(bytes, mimeType);
        }

        #endregion
    }
}