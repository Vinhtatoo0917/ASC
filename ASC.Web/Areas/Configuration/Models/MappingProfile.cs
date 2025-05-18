using ASC.Model.Models;
using AutoMapper;
using MathNet.Numerics.LinearAlgebra;

namespace ASC.Web.Areas.Configuration.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            CreateMap<MasterDataKey, MasterDataKeyViewModel>();
            CreateMap<MasterDataKeyViewModel, MasterDataKey>();
            CreateMap<MasterDataValue, MasterDataValueViewModel>();
            CreateMap<MasterDataValueViewModel, MasterDataValue>();
        }
    }
}
