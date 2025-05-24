using System;
using System.Diagnostics.CodeAnalysis;

namespace AdPlatforms.Common;

public readonly struct Result<TValue, TError>
{
	private readonly TValue? _value;
	private readonly TError? _error;
	
	[MemberNotNullWhen(true, nameof(_value))]
	[MemberNotNullWhen(false, nameof(_error))]
	public bool IsSuccess { get; }
	
	private Result(bool isSuccess, TValue? value, TError? error)
		=> (IsSuccess, _value, _error) = (isSuccess, value, error);
	
	public bool IsOk([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out TError error)
	{
		value = _value;
		error = _error;
		return IsSuccess;
	}
	
	public bool IsError([MaybeNullWhen(false)] out TError error, [MaybeNullWhen(true)] out TValue value)
		=> !IsOk(out value, out error);
	
	public T Match<T>(Func<TValue, T> success, Func<TError, T> failure)
		=> IsSuccess ? success.Invoke(_value) : failure.Invoke(_error);
	
	public static Result<TValue, TError> Success(TValue value) => new(true, value, default);
	public static Result<TValue, TError> Failure(TError error) => new(false, default, error);
	
	public static implicit operator Result<TValue, TError>(TValue value) => new(true, value, default);
	public static implicit operator Result<TValue, TError>(TError error) => new(false, default, error);
}