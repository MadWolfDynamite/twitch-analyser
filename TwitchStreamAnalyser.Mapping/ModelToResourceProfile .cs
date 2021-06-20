using AutoMapper;
using System;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<TwitchAccount, TwitchAccountResource>();
        }
    }
}
