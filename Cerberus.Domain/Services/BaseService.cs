using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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


        public async Task<IEnumerable<TDto>> Get(Dictionary<string, dynamic> where = null)
        {
            return _mapper.Map<IEnumerable<TDto>>(await _repository.Get(where: where));
        }

        public async Task<TDto> GetById(object id)
        {
            return _mapper.Map<TDto>(await _repository.GetById(id));
        }

        public async Task<TDto> Save<TP>(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var id = await _repository.Save<TP>(entity);
            return _mapper.Map<TDto>(await _repository.GetById(id));
        }

        public async Task<IEnumerable<TDto>> SaveRange<TP>(IEnumerable<TDto> dto)
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(dto);
            var ids = await _repository.SaveRange<TP>(entities);
            var createdEntities = await _repository.GetIn(data: ids.ToArray(), inferPk: true);
            return _mapper.Map<IEnumerable<TDto>>(createdEntities);
        }

        public async Task Update(TDto dto)
        {
            await _repository.Update(_mapper.Map<TEntity>(dto));
        }

        public async Task UpdateRange(IEnumerable<TDto> dto)
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(dto);
            await _repository.UpdateRange(entities);
        }

        public async Task Delete(TDto dto)
        {
            await _repository.Delete(_mapper.Map<TEntity>(dto));
        }

        public async Task DeleteRange(IEnumerable<TDto> dto)
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(dto);
            await _repository.DeleteRange(entities);
        }

        public async Task<TDto> Create<TP>(TDto dto)
        {
            return await Save<TP>(dto);
        }
    }
}