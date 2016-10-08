using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using Persistence.Dao;
using Persistence.Mappers;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

       protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }
            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(x => x.UserInfromation)
                .WithOptional()
                .Map(x => x.MapKey("UserExtendedId"));
           modelBuilder.Entity<UserExtended>()
               .HasMany(z => z.Modules)
               .WithOptional(m => m.User)
               .Map(z => z.MapKey("UserId"));
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
           modelBuilder.Configurations.Add(new BillMap());
           modelBuilder.Configurations.Add(new BillTypeMap());
           modelBuilder.Configurations.Add(new ClientMap());
           modelBuilder.Configurations.Add(new InboundMap());
           modelBuilder.Configurations.Add(new InboundtrackingMap());
           modelBuilder.Configurations.Add(new MeasureUnitMap());
           modelBuilder.Configurations.Add(new PackageTypeMap());
           modelBuilder.Configurations.Add(new ProductMap());
           modelBuilder.Configurations.Add(new ProviderMap());
           modelBuilder.Configurations.Add(new TaxConditionMap());
           modelBuilder.Configurations.Add(new UbicationMap());
           modelBuilder.Configurations.Add(new ShipmentMap());
           modelBuilder.Configurations.Add(new ShipmentTrackMap());
           modelBuilder.Configurations.Add(new RolesMap());
           modelBuilder.Configurations.Add(new ModuleMap());
           modelBuilder.Configurations.Add(new ActionsMap());
           modelBuilder.Configurations.Add(new ModuleActionsMap());
           modelBuilder.Configurations.Add(new ModuleUserActionsMap());
           modelBuilder.Configurations.Add(new MessagingMap());
           modelBuilder.Configurations.Add(new AlertMap());
           modelBuilder.Configurations.Add(new NotificationMap());
           modelBuilder.Configurations.Add(new ServicesMap());
           modelBuilder.Configurations.Add(new ContractTemplateMap());
           modelBuilder.Configurations.Add(new ContractMap());
           modelBuilder.Configurations.Add(new CarMap());
           modelBuilder.Configurations.Add(new AlmacenMap());
           modelBuilder.Configurations.Add(new ComentariosMap());
           modelBuilder.Configurations.Add(new ImagenesMap());
           modelBuilder.Configurations.Add(new PreguntasMap());
           modelBuilder.Configurations.Add(new PromocionesMap());
           modelBuilder.Configurations.Add(new StockMap());
           modelBuilder.Configurations.Add(new EmpresaMap());
           modelBuilder.Configurations.Add(new CreditoMap());
           modelBuilder.Configurations.Add(new CuotasMap());
           modelBuilder.Configurations.Add(new CuentasCobrarMap());
           modelBuilder.Configurations.Add(new CuentasPagarMap());
           modelBuilder.Configurations.Add(new BancariaMap());
           modelBuilder.Configurations.Add(new PedidosMap());
           modelBuilder.Configurations.Add(new VentaMap());
           modelBuilder.Configurations.Add(new PedidosProductoMap());
           
        }




        //public static ApplicationDbContext Create()
        //{
        //    return System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        //}
    }
}
