#nullable enable

namespace GoopBenchmark
{
	using BenchmarkDotNet.Attributes;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Tests whether its faster to use the version of <c>string.Concat</c> that takes an array or an enumerable.
	/// </summary>
	public class StringConcat
	{
		[Benchmark]
		public string UsingArray()
		{
			int numLines = 0;
			string[] lines = new string[4];

			if (this.AttachmentNarrowingType != null)
			{
				lines[numLines++] = $@"<br/>
			/// This attached property is only for use with objects of type <typeparamref name=""__TTarget""/>.";
			}

			if (this.ValidationMethodName != null)
			{
				lines[numLines++] = $@"<br/>
			/// Uses <see cref=""{this.ValidationMethodName}""/> for validation.";
			}

			if (this.CoercionMethodName != null)
			{
				lines[numLines++] = $@"<br/>
			/// Uses <see cref=""{this.CoercionMethodName}""/> for coercion.";
			}

			if (this.ChangedHandlerName != null)
			{
				lines[numLines++] = $@"<br/>
			/// Uses <see cref=""{this.ChangedHandlerName}""/> for changes.";
			}

			return string.Concat(lines);
		}

		[Benchmark]
		public string UsingList()
		{
			var lines = new List<string>(4);

			if (this.AttachmentNarrowingType != null)
			{
				lines.Add($@"<br/>
			/// This attached property is only for use with objects of type <typeparamref name=""__TTarget""/>.");
			}

			if (this.ValidationMethodName != null)
			{
				lines.Add($@"<br/>
			/// Uses <see cref=""{this.ValidationMethodName}""/> for validation.");
			}

			if (this.CoercionMethodName != null)
			{
				lines.Add($@"<br/>
			/// Uses <see cref=""{this.CoercionMethodName}""/> for coercion.");
			}

			if (this.ChangedHandlerName != null)
			{
				lines.Add($@"<br/>
			/// Uses <see cref=""{this.ChangedHandlerName}""/> for changes.");
			}

			return string.Concat(lines);
		}

		private Type AttachmentNarrowingType { get; } = typeof(object);
		private string? ValidationMethodName { get; } = "IsValidFoo";
		private string? CoercionMethodName { get; }
		private string? ChangedHandlerName { get; } = "OnFooPropertyChanged";
	}
}
