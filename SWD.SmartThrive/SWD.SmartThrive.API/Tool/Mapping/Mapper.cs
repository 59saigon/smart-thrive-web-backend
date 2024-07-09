using AutoMapper;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.API.Tool.Mapping
{
    public partial class Mapper
    {
        public Mapper()
        {
            CategoryMapping();
            CourseMapping();
            OrderMapping();
            PackageMapping();
            ProviderMapping();
            SessionMapping();
            StudentMapping();
            UserMapping();
            CourseXPackageMapping();
            SubjectMapping();
        }
    }
}
