using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Ports;
using Microsoft.AspNetCore.Mvc;

namespace Intecgra.Cerberus.Api.Controllers
{
    public abstract class BaseController<TDto> : ControllerBase
        where TDto : class, new()
    {
        private readonly IBaseService<TDto> _service;

        public BaseController(IBaseService<TDto> service)
        {
            _service = service;
        }
    }
}