﻿using SmartThrive.DataAccess.Repositories.Base;
using SmartThrive.DataAccess.Repositories.Repositories.Interface;
using SmartThrive.DataAccesss.Repositories.Repositories.Interface;
using ST.Entities.Data;
using ST.Entities.Data.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartThrive.DataAccess.Repositories.Repositories
{
    public class CourseXPackageRepository : BaseRepository<Provider>, ICourseXPackageRepository
    {
        public CourseXPackageRepository(STDbContext context) : base(context)
        {
        }
    }
}
