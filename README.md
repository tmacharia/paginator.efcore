# Paginator.EntityFrameworkCore 

[![Build](https://github.com/tmacharia/paginator.efcore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tmacharia/paginator.efcore/actions/workflows/dotnet.yml)
[![Nuget](https://img.shields.io/nuget/vpre/Paginator.EntityFrameworkCore.svg?logo=nuget&link=https://www.nuget.org/packages/Paginator.EntityFrameworkCore/left)](https://www.nuget.org/packages/Paginator.EntityFrameworkCore)

Asynchronous & Synchronous pagination of queries to your data store using EntityFrameworkCore. With the help of some simple to use extension methods, querying portions/chunks of data just gets easier.

## Use Cases

Suppose we have 5,000 rows in the `Employee` table, lets look at a few different ways to query chunks of that data as efficiently as possible and return the result in a paginated format.

This code snippet represents our connection to the table whilst telling EF not to track the results of the query, inorder to get some slight performance improvement.

```csharp 
IQueryable<Employee> employees = _context.Set<Employee>().AsNoTracking(); 
```


### Example-I: Page 2, Perpage 20
Internally translates to skipping the first page and taking the next 20 records that follow.
```csharp
PagedResult<Employee> paged = employees.Paginate(2,20);
Console.WriteLine(paged);
// Page: 2 Perpage: 20 Totalpages: 250 TotalItems: 5,000
```
### Example-II: Page 2, Perpage 20, Asynchronously
`await` call to `.PaginateAsync` and optionally pass a `CancellationToken` as shown below. The result of this query will be the same as that of the previous example.

An `OperationCanceledException` is thrown if cancellation is requested on the specified cancellationToken.

```csharp
var tokenSource = new CancellationTokenSource();

PagedResult<Employee> paged = await employees.PaginateAsync(2,20, token: tokenSource.Token);

Console.WriteLine(paged);
// Page: 2 Perpage: 20 Totalpages: 250 TotalItems: 5,000
```

## Skip count & perfomance concerns of `.Count()`

We all know that pagination is a two step procedure, with one counting the total number of items in a sequence, and the second that picks N items from that sequence.

Sometimes, the `.Count()` query doesn't perform very well especially when dealing with large datasets, or your use case might not require knowing `TotalPages` and `TotalItems`. We can tell the pagination method to ignore doing the count operation as illustrated in the examples below.


### Example-III: Page 2, Perpage 20, Skipping Count
To ignore count, just pass `true` as a parameter to the `param: skipCount` which comes right after perpage.

Take a look at the console output to spot the difference.
```csharp
PagedResult<Employee> paged = employees.Paginate(2,20, skipCount: true);
Console.WriteLine(paged);
// Page: 2 Perpage: 20 Totalpages: 1 TotalItems: 20
```

## Contributions

Suggestions & improvements are welcome.