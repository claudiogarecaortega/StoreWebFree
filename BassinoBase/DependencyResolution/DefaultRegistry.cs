// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using BassinoBase.Models;
using Domain.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Persistence;
using Persistence.Dao;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using StructureMap;
using Utils;
using WebGrease;

namespace BassinoBase.DependencyResolution {
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors
        public static IList<string> Assemblies
        {
            get
            {
                return new List<string>
                {
                    "BassinoBase",
                    "Persistence"
                };
            }
        }

        public static IList<Tuple<string, string>> ManuallyWired
        {
            get
            {
                return new List<Tuple<string, string>>()
                {
                   Tuple.Create("IUserStore<ApplicationUser>", "UserStore<ApplicationUser>>"),
                    Tuple.Create("DbContext", "ApplicationDbContext"),
                    Tuple.Create("IAuthenticationManager", "HttpContext.Current.GetOwinContext().Authentication"),
                };
            }
        }
        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });
            //For<IExample>().Use<Example>();
            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            //For<Iapplic>().Use(() => System.Web.HttpContext.Current.GetOwinContext().Authentication);
            For<IAuthenticationManager>().Use(() => System.Web.HttpContext.Current.GetOwinContext().Authentication);
            For<DbContext>().Use(() => System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>());
            For<IActivatorWrapper>().Use<ActivatorWrapper>();
            For<IUnitOfWorkHelper>().Use<UnitOfWorkHelper>();
            //this.For<IHttpHandler>().Use<UploadHandler>();
            //this.For<IHttpHandler>().Use<UploadProduct>();

        }

        #endregion
    }
    public class InterfazRegistry : Registry
    {
        public InterfazRegistry()
        {
            this.Scan(x =>
            {
                x.TheCallingAssembly();
                x.IncludeNamespaceContainingType<LogManager>();
                x.IncludeNamespaceContainingType<Controller>();
                x.Assembly("BassinoBase");
                //x.Assembly("BassinoBase.Controllers");
                x.WithDefaultConventions();


            });
           // this.For<IControllerBehabior>().Use<ControllerBehabior>();
          //  this.For<IContactViewModelMapper>().Use<ContactViewModelMapper>();
           
        }
    }
    public class DAORegistry : Registry
    {
        public DAORegistry()
        {

            this.Scan(x =>
            {
                x.AssemblyContainingType<ApplicationDbContext>();
                x.WithDefaultConventions();
            });
         //   For<ICategoryFoodDao>().Use<CategoryFoodDao>();
              For<IUserdDao>().Use<UserDao>();
              For<IBillDao>().Use<BillDao>();
              For<IBillTypeDao>().Use<BillTypeDao>();
              For<IClientDao>().Use<ClientDao>();
              For<IInboundDao>().Use<InboundDao>();
              For<IInboundtrackingDao>().Use<InboundtrackingDao>();
              For<IMeasureUnitDao>().Use<MeasureUnitDao>();
              For<IPackageTypeDao>().Use<PackageTypeDao>();
              For<IProviderDao>().Use<ProviderDao>();
              For<IProductDao>().Use<ProductDao>();
              For<ITaxConditionDao>().Use<TaxConditionDao>();
              For<IUbicationDao>().Use<UbicationDao>();
              For<IShipmentDao>().Use<ShipmentDao>();
              For<IShipmentTrackDao>().Use<ShipmentTrackDao>();
        }
    }
    public class UowRegistry : Registry
    {
        public UowRegistry()
        {
            this.For<IUnitOfWorkHelper>()
                //.HttpContextScoped()
                .Use<UnitOfWorkHelper>()
                .OnCreation("", i => i._sessionContext = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>());

        }

        private void BuildUpEntitiesHandler(object sender, ObjectCreatedEventArgs args)
        {
            ObjectFactory.Container.BuildUp(args.Entity);
        }
    }
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {
            this.For<IActivatorWrapper>()
                //.HttpContextScoped()
                .Use<ActivatorWrapper>()
                .OnCreation("", i => i.ObjectCreated += BuildUpEntitiesHandler);
        }

        private void BuildUpEntitiesHandler(object sender, ObjectCreatedEventArgs args)
        {
            ObjectFactory.Container.BuildUp(args.Entity);
        }
    }
}