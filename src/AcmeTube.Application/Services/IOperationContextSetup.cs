using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Services;

public interface IOperationContextSetup
{
	void Setup(OperationContext context);
}