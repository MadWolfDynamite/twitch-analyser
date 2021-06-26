using AutoMapper;
using System;
using TwitchStreamAnalyser.Api.Resources;
using TwitchStreamAnalyser.Domain.Models;

namespace TwitchStreamAnalyser.Api.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<TwitchAccount, TwitchAccountResource>();
            CreateMap<TwitchChannel, TwitchChannelResource>();
            CreateMap<TwitchStream, TwitchStreamResource>();
        }
    }
}
