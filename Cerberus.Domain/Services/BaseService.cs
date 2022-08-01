using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Domain.Exceptions;
using Cerberus.Domain.Ports;
using Cerberus.Domain.Ports.Repository;

namespace Cerberus.Domain.Services
{
    public abstract class BaseService<TEntity, TDto> : IBaseService<TDto>
        where TEntity : class where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        protected BaseService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAll() => _mapper.Map<IEnumerable<TDto>>(await _repository.GetAll());

        public async Task<TDto> GetById(object id) => _mapper.Map<TDto>(await _repository.GetById(id));

        public async Task<TK> Create<TK>(TDto dto) => await _repository.Create<TK>(_mapper.Map<TEntity>(dto));

        public async Task Create(IEnumerable<TDto> dtos) =>
            await _repository.Create(_mapper.Map<IEnumerable<TEntity>>(dtos));

        public async Task<bool> Update(TDto dto) => await _repository.Update(_mapper.Map<TEntity>(dto));

        public async Task<bool> Update(IEnumerable<TDto> dtos) =>
            await _repository.Update(_mapper.Map<IEnumerable<TEntity>>(dtos));

        public async Task<bool> Delete(int id)
        {
            var success = await _repository.Delete(id);
            if (!success) throw new DomainException("El registro no pudo ser eliminado.");
            return success;
        }
    }
}