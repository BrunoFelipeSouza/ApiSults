namespace ApiSults.Infrastructure.Services.RefreshData.Responses;

public sealed record class GetTicketsResponse
{
    public required IEnumerable<Ticket> Data { get; init; }
    public required int Start { get; init; }
    public required int Limit { get; init; }
    public required int Size { get; init; }
    public required int TotalPage { get; init; }
}

public sealed record class Ticket
{
    public required long Id { get; init; }
    public required string Titulo { get; init; }
    public required IdNome Solicitante { get; init; }
    public required IdNome Responsavel { get; init; }
    public required IdNome Unidade { get; init; }
    public required IdNome Departamento { get; init; }
    public required IdNome Assunto { get; init; }
    public IEnumerable<Apoio>? Apoio { get; init; }
    public required IEnumerable<Etiqueta> Etiqueta { get; init; }
    public required int Tipo { get; init; }
    public required DateTime Aberto { get; init; }
    public required DateTime? Resolvido { get; init; }
    public required DateTime? Concluido { get; init; }
    public required DateTime ResolverPlanejado { get; init; }
    public required DateTime ResolverEstipulado { get; init; }
    public required int? AvaliacaoNota { get; init; }
    public required string? AvaliacaoObservacao { get; init; }
    public required int Situacao { get; init; }
    public DateTime? PrimeiraInteracao { get; init; }
    public required DateTime UltimaAlteracao { get; init; }
    public required int CountInteracaoPublico { get; init; }
    public required int CountInteracaoInterno { get; init; }
}

public sealed record class IdNome
{
    public required long? Id { get; init; }
    public required string? Nome { get; init; }
}

public sealed record class Apoio
{
    public IdNome? Pessapoiooa { get; init; }
    public IdNome? Departamento { get; init; }
    public bool PessoaUnidade { get; init; }
}

public sealed record class Etiqueta
{
    public required long Id { get; init; }
    public required string Nome { get; init; }
    public required string Cor { get; init; }
}