using AutoMapper;
using Iot.Max.Model.Dtos;
using Iot.Max.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            //CreateMap<RetailBrand, RetailBrand>();

            //CreateMap<RetailSPU,RetailSPUDto>();

            CreateMap<InterQuestionCategory, InterQuestionCategoryDto>();

        }
    }
}
