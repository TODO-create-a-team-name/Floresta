using AutoMapper;
using Floresta.Models;
using Floresta.ViewModels;

namespace Floresta.Mappers
{
    public class QuestionTopicProfile : Profile
    {
        public QuestionTopicProfile()
        {
            CreateMap<QuestionTopic, QuestionTopicViewModel>()
                .ForMember(dest =>
                dest.Topic,
                opt => opt.MapFrom(src => src.Topic))
                .ReverseMap();
        }
    }
}
