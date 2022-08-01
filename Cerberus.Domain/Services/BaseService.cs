using System.Collections.Generic;
using System.Threading.Tasks;
using Cerberus.Domain.Exceptions;
using Cerberus.Domain.Ports;
using Cerberus.Domain.Ports.Repository;
using Mapster;

namespace Cerberus.Domain.Services;

public abstract class BaseService<TEntity, TDto> : IBaseService<TDto>
    where TEntity : class where TDto : class
{
    private readonly IGenericRepository<TEntity> _repository;

    protected BaseService(IGenericRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TDto>> GetAll()
    {
        return (await _repository.GetAll()).Adapt<IEnumerable<TDto>>();
    }

    public async Task<TDto> GetById(object id)
    {
        return (await _repository.GetById(id)).Adapt<TDto>();
    }

    public async Task<TK> Create<TK>(TDto dto)
    {
        return await _repository.Create<TK>(dto.Adapt<TEntity>());
    }

    public async Task Create(IEnumerable<TDto> dtos)
    {
        await _repository.Create(dtos.Adapt<IEnumerable<TEntity>>());
    }

    public async Task<bool> Update(TDto dto)
    {
        return await _repository.Update(dto.Adapt<TEntity>());
    }

    public async Task<bool> Delete(int id)
    {
        var success = await _repository.Delete(id);
        if (!success) throw new DomainException("El registro no pudo ser eliminado.");
        return success;
    }

    public async Task<bool> Update(IEnumerable<TDto> dtos)
    {
        return await _repository.Update(dtos.Adapt<IEnumerable<TEntity>>());
    }
}