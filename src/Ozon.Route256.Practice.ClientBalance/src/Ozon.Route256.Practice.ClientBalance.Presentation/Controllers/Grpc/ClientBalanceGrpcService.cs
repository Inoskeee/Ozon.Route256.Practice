using Google.Protobuf.Collections;
using Google.Type;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Services;
using Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Controllers.Grpc;

public sealed class ClientBalanceGrpcService : ClientBalanceGrpc.ClientBalanceGrpcBase
{
    private readonly ILogger<ClientBalanceGrpcService> _logger;
    
    private readonly IClientBalanceService _clientBalanceService;

    private readonly IClientRequestMapper _clientRequestMapper;
    private readonly IChangeOperationRequestMapper _changeOperationRequestMapper;
    private readonly IRegisterOperationRequestMapper _registerOperationRequestMapper;
    public ClientBalanceGrpcService(
        ILogger<ClientBalanceGrpcService> logger, 
        IClientBalanceService clientBalanceService, 
        IClientRequestMapper clientRequestMapper, 
        IChangeOperationRequestMapper changeOperationRequestMapper, 
        IRegisterOperationRequestMapper registerOperationRequestMapper)
    {
        _logger = logger;
        _clientBalanceService = clientBalanceService;
        _clientRequestMapper = clientRequestMapper;
        _changeOperationRequestMapper = changeOperationRequestMapper;
        _registerOperationRequestMapper = registerOperationRequestMapper;
    }

    public override async Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
    {
        _logger.LogInformation("CreateClient request: {createClientRequest}", request);

        var clientModel = _clientRequestMapper.MapDtoToModel(request);
        
        var result = await _clientBalanceService.CreateClientAsync(clientModel, context.CancellationToken);
        
        _logger.LogInformation("CreateClient result: {createClientResult}", result);

        return result.IsSuccess ? new CreateClientResponse()
        {
            Ok = new CreateClientResponse.Types.CreateClientSuccess()
            {
                Message = result.ActionSuccessEntity?.SuccessContent
            }
        } : new CreateClientResponse()
        {
            Error = new CreateClientResponse.Types.CreateClientError()
            {
                Code = result.ErrorEntity?.ErrorCode,
                Message = result.ErrorEntity?.ErrorMessage
            }
        };
    }

    public override async Task<TopUpBalanceResponse> TopUpBalance(TopUpBalanceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("TopUpBalance request: {topUpRequest}", request);

        var registerModel = _registerOperationRequestMapper.MapTopUpRequestToModel(request);
        
        var result = await _clientBalanceService.TopUpBalanceAsync(
            registerModel,
            context.CancellationToken);
        
        _logger.LogInformation("TopUpBalance result: {topUpResult}", result);

        return result.IsSuccess ? new TopUpBalanceResponse()
        {
            Ok = new TopUpBalanceResponse.Types.TopUpBalanceSuccess()
            {
                Message = result.ActionSuccessEntity?.SuccessContent
            }
        } : new TopUpBalanceResponse()
        {
            Error = new TopUpBalanceResponse.Types.TopUpBalanceError()
            {
                Code = result.ErrorEntity?.ErrorCode,
                Message = result.ErrorEntity?.ErrorMessage
            }
        };
    }

    public override async Task<WithdrawBalanceResponse> WithdrawBalance(WithdrawBalanceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("WithdrawBalance request: {withdrawBalanceRequest}", request);

        var registerModel = _registerOperationRequestMapper.MapWithdrawRequestToModel(request);
        
        var result = await _clientBalanceService.WithdrawBalanceAsync(
            registerModel,
            context.CancellationToken);
        
        _logger.LogInformation("WithdrawBalance result: {withdrawBalanceResult}", result);

        return result.IsSuccess ? new WithdrawBalanceResponse()
        {
            Ok = new WithdrawBalanceResponse.Types.WithdrawBalanceSuccess()
            {
                Message = result.ActionSuccessEntity?.SuccessContent
            }
        } : new WithdrawBalanceResponse()
        {
            Error = new WithdrawBalanceResponse.Types.WithdrawBalanceError()
            {
                Code = result.ErrorEntity?.ErrorCode,
                Message = result.ErrorEntity?.ErrorMessage
            }
        };
    }

    public override async Task<ChangeOperationStatusResponse> ChangeOperationStatus(ChangeOperationStatusRequest request, ServerCallContext context)
    {
        _logger.LogInformation("ChangeOperationStatus request: {topUpRequest}", request);

        var changeOperationModel = _changeOperationRequestMapper.MapChangeOperationRequestToModel(request);
        
        var result = await _clientBalanceService.ChangeOperationStatusAsync(
            changeOperationModel,
            context.CancellationToken);
        
        _logger.LogInformation("ChangeOperationStatus result: {topUpResult}", result);

        return result.IsSuccess ? new ChangeOperationStatusResponse()
        {
            Ok = new ChangeOperationStatusResponse.Types.ChangeOperationStatusSuccess()
            {
                Message = result.ActionSuccessEntity?.SuccessContent
            }
        } : new ChangeOperationStatusResponse()
        {
            Error = new ChangeOperationStatusResponse.Types.ChangeOperationStatusError()
            {
                Code = result.ErrorEntity?.ErrorCode,
                Message = result.ErrorEntity?.ErrorMessage
            }
        };
    }

    public override async Task<GetCurrentBalanceResponse> GetCurrentBalance(GetCurrentBalanceRequest request, ServerCallContext context)
    {
        _logger.LogInformation("GetCurrentBalance request: {topUpRequest}", request);

        var result = await _clientBalanceService.GetCurrentBalanceAsync(
            request.ClientId, 
            context.CancellationToken);
        
        _logger.LogInformation("GetCurrentBalance result: {topUpResult}", result);
        
        return result.IsSuccess ? new GetCurrentBalanceResponse()
        {
            Ok = new GetCurrentBalanceResponse.Types.GetCurrentBalanceSuccess()
            {
                CurrentBalance = 
                { 
                    new RepeatedField<Money>()
                    {
                        result.ActionSuccessEntity!.SuccessContent.Select(entity => new Money()
                        {
                            CurrencyCode = entity.CurrencyCode,
                            Nanos = entity.Nanos,
                            Units = entity.Units
                        })
                    }
                }
            }
        } : new GetCurrentBalanceResponse()
        {
            Error = new GetCurrentBalanceResponse.Types.GetCurrentBalanceError()
            {
                Code = result.ErrorEntity?.ErrorCode,
                Message = result.ErrorEntity?.ErrorMessage
            }
        };
    }
}