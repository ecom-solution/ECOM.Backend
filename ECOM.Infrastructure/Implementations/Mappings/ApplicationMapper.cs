using AutoMapper;
using ECOM.Domain.Interfaces.Mappings;

namespace ECOM.Infrastructure.Implementations.Mappings
{
	public class ApplicationMapper(IMapper mapper) : IMap
	{
		private readonly IMapper _mapper = mapper;

		/// <inheritdoc/>
		public TDestination Map<TDestination>(object source)
		{
			return _mapper.Map<TDestination>(source);
		}

		/// <inheritdoc/>
		public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
		{
			return _mapper.Map(source, destination);
		}

		/// <inheritdoc/>
		public IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
		{
			return _mapper.Map<IEnumerable<TDestination>>(source);
		}
	}
}
