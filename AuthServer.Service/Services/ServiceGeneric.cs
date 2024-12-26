using AuthServer.Core.Repositories;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Sharedlayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _repository;
        public ServiceGeneric(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var newentity = ObjectMapper.Mapper.Map<TEntity>(dto);
            await _repository.AddAsync(newentity);
            await _unitOfWork.CommitAsync();
            var newdto = ObjectMapper.Mapper.Map<TDto>(newentity);
            return Response<TDto>.Success(newdto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var listentity = await _repository.GetAllAsyncEnumerable();
            var dtolist = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(listentity);
            return Response<IEnumerable<TDto>>.Success(dtolist,200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity =await _repository.GetByIdAsync(id);
            if (entity==null)
            {
                Response<TDto>.Fail("Id not found", 404, true);
            }
            var dto = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(dto, 200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isexsistentity = await _repository.GetByIdAsync(id);
            if (isexsistentity==null)
            {
                return Response<NoDataDto>.Fail("Not Found", 404, true);
            }
            //var entity = ObjectMapper.Mapper.Map<TEntity>(dto);
            _repository.Remove(isexsistentity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);

        }

        public async Task<Response<NoDataDto>> Update(TDto dto,int id)
        {
            var isexsistentity = await _repository.GetByIdAsync(id);
            if (isexsistentity == null)
            {
                return Response<NoDataDto>.Fail("Not Found", 404, true);
            }

            var entity = ObjectMapper.Mapper.Map<TEntity>(dto);
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _repository.Where(predicate);
            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);

        }
    }
}
