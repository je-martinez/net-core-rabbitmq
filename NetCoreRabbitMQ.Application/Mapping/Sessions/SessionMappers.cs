using AutoMapper;
using NetCoreRabbitMQ.Domain.Entities;
using NetCoreRabbitMQ.Application.DTOs.Sessions;

namespace NetCoreRabbitMQ.Application.Mapping
{
    public static class SessionMappers
    {
        private static MapperConfiguration configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Session, SessionDTO>().ReverseMap();
            cfg.CreateMap<CreateSessionDTO, Session>().ReverseMap();
            cfg.CreateMap<CreateSessionDTO, SessionDTO>().ReverseMap();


        });

        static Mapper mapper = new Mapper(configuration);

        public static SessionDTO ToSessionDTO(this Session Session)
        {
            return mapper.Map<SessionDTO>(Session);
        }

        public static SessionDTO ToSessionDTO(this CreateSessionDTO Session)
        {
            return mapper.Map<SessionDTO>(Session);
        }


        public static Session ToSession(this SessionDTO SessionDTO)
        {
            return mapper.Map<Session>(SessionDTO);
        }

        public static Session ToSession(this CreateSessionDTO SessionDTO)
        {
            return mapper.Map<Session>(SessionDTO);
        }
    }
}
