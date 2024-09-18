# Sidub Platform - Storage

This repository contains the storage module for the Sidub Platform. It provides
create, read, update, delete and related capabilities against entity concepts.

Abstractions and core concepts are defined within this library and
implementations against specific data sources are provided in the respective
storage library (i.e., Sidub.Platform.Storage.Http for web based data sources).

## Main Components
The library defines abstractions and services that provide data source
connectivity for the entity, entity relationship, etc. concepts defined within
the platform.

### Registering a Data Service
This library provides the necessary services and abstractions only; see
individual implementations for directions on the supported connectors and
service registration schemes.

### Querying Entities
Entity queries are defined as query objects by inheriting either the
`IRecordQuery<TResult>` or `IEnumerableQuery<TResult>` interfaces. The
`IRecordQuery<TResult>` interface is used for queries that return a single
result, while the `IEnumerableQuery<TResult>` interface is used for queries
that return multiple results.

Query objects are defined with the properties that compose the given query;
required query parameters should be accepted in the constructor while optional
parameters are assigned defaults. Each query object must implement the
`IFilter? GetFilter()` method which is responsible for building a filter
statement leveraging the parameter values within the query object.

---csharp
public class SalesOrderByOrderNumberQuery : IRecordQuery<SalesOrder>
{
    public string OrderNumber { get; }

    public SalesOrderByOrderNumberQuery(string orderNumber)
    {
        OrderNumber = orderNumber;
    }

    public IFilter? GetFilter()
    {
        var filterBuilder = new FilterBuilder();
        filterBuilder.Add("OrderNumber", ComparisonOperator.Equals, Id);

        var filter = filterBuilder.Build();
        return filter;
    }
}
---

Queries are executed by calling the `Execute` method on the `IQueryService`
service, providing the query to execute and the service reference to execute
against.

---csharp
public class OrderService
{
    private readonly StorageServiceReference _serviceReference = new
    StorageServiceReference("MyApi");
    private readonly IQueryService _queryService;

    public SalesOrderService(IQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<SalesOrder> GetSalesOrderByOrderNumber(string orderNumber)
    {
        var query = new SalesOrderByOrderNumberQuery(orderNumber);
        var salesOrder = await _queryService.Execute(_serviceReference, query);
        return salesOrder;
    }
}
---

### Saving Entities
Entities are saved by using the `SaveEntityCommand<TEntity>` command object.
The command object may be initialized by using the SaveEntityCommand.Create()
method, passing the entity to save. To execute the save command, call the
`Execute` method on the `IQueryService` service, providing the command to
execute and the service reference to execute against. A `SaveEntityResponse<TEntity>`
will be returned denoting whether the save was successful and providing the saved
entity (particularly useful when the save operation may have modified or generated
additional entity properties).

---csharp
public class OrderService
{
    private readonly StorageServiceReference _serviceReference = new
    StorageServiceReference("MyApi");
    private readonly ICommandService _commandService;

    public SalesOrderService(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task<SalesOrder> SaveSalesOrder(SalesOrder salesOrder)
    {
        var command = SaveEntityCommand.Create(salesOrder);
        var saveResult = await _commandService.Execute(_serviceReference, command);

        if (!saveResult.IsSuccessful)
            throw new Exception("Error encountered during order save.");

        return saveResult.Result;
    }
}
---

### Deleting Entities
Entities are deleted using the `DeleteEntityCommand<TEntity>` command object.
The command object may be initialized by using the DeleteEntityCommand.Create()
method, passing the entity to delete. To execute the delete command, call the
`Execute` method on the `IQueryService` service, providing the command to execute
and the service reference to execute against. A `DeleteEntityResponse` will be
returned denoting whether the delete was successful.

---csharp
public class OrderService
{
    private readonly StorageServiceReference _serviceReference = new
    StorageServiceReference("MyApi");
    private readonly ICommandService _commandService;

    public SalesOrderService(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task DeleteSalesOrder(SalesOrder salesOrder)
    {
        var command = DeleteEntityCommand.Create(salesOrder);
        var deleteResult = await _commandService.Execute(_serviceReference, command);

        if (!deleteResult.IsSuccessful)
            throw an Exception("Error encountered during order delete.");
    }
}
---

### Entity Relationships
> [!WARNING]
> Relationship functionality is functional but currently considered in beta;
> certain scenarios may not be fully handled.

The storage framework leverages the platform's concept of entity relationships
and supports the saving and querying of data through these relationships.
Additional documentation will be added here in the future.

## Extensibility
A variety of concrete data source implementations have been provided and the
framework may be extended to support any type of data source / connectivity
requirement. Additional documentation will be added here in the future.

## License
This project is dual-licensed under the AGPL v3 or a proprietary license. For
details, see [https://sidub.ca/licensing](https://sidub.ca/licensing) or the
LICENSE.txt file.