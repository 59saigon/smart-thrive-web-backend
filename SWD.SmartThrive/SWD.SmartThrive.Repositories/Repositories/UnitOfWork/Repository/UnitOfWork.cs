using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Repository
{
    public class UnitOfWork : BaseUnitOfWork<STDbContext>,IUnitOfWork
    {
        public UnitOfWork(STDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }

        public ICategoryRepository CategoryRepository => GetRepository<ICategoryRepository>();

        public ICourseRepository CourseRepository => GetRepository<ICourseRepository>();

        public ICourseXPackageRepository CourseXPackageRepository => GetRepository<ICourseXPackageRepository>();

        public IOrderRepository OrderRepository => GetRepository<IOrderRepository>();

        public IPackageRepository PackageRepository => GetRepository<IPackageRepository>();

        public IRoleRepository RoleRepository => GetRepository<IRoleRepository>();

        public ISessionRepository SessionRepository => GetRepository<ISessionRepository>();

        public IStudentRepository StudentRepository => GetRepository<IStudentRepository>();

        public IUserRepository UserRepository => GetRepository<IUserRepository>();

        public IProviderRepository ProviderRepository => GetRepository<IProviderRepository>();

        public ISubjectRepository SubjectRepository => GetRepository<ISubjectRepository>();

    }
}
