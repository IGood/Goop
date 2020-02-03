using BenchmarkDotNet.Attributes;
using Goop;
using System;
using System.Text;

namespace GoopBenchmark
{
	public class StringBuilderFormatting
	{
		private int a = 1;
		private float b = 2;
		private string c = "3";
		private object d = new Exception("ex");
		private DateTime e = DateTime.Now;

		[Benchmark]
		public StringBuilder AppendInterpolatedString1 ()
		{
			var builder = new StringBuilder();
			builder.Append($"- {a} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendInterpolatedString2 ()
		{
			var builder = new StringBuilder();
			builder.Append($"- {a} - {b} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendInterpolatedString3 ()
		{
			var builder = new StringBuilder();
			builder.Append($"- {a} - {b} - {c} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendInterpolatedString4 ()
		{
			var builder = new StringBuilder();
			builder.Append($"- {a} - {b} - {c} - {d} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendInterpolatedString5 ()
		{
			var builder = new StringBuilder();
			builder.Append($"- {a} - {b} - {c} - {d} - {e} - ");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormat1 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("- {0} -", a);
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormat2 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("- {0} - {1} -", a, b);
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormat3 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("- {0} - {1} - {2} -", a, b, c);
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormat4 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("- {0} - {1} - {2} - {3} -", a, b, c, d);
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormat5 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("- {0} - {1} - {2} - {3} - {4} -", a, b, c, d, e);
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormatted1 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormatted($"- {a} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormatted2 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormatted($"- {a} - {b} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormatted3 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormatted($"- {a} - {b} - {c} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormatted4 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormatted($"- {a} - {b} - {c} - {d} -");
			_ = builder.ToString();
			return builder;
		}

		[Benchmark]
		public StringBuilder AppendFormatted5 ()
		{
			var builder = new StringBuilder();
			builder.AppendFormatted($"- {a} - {b} - {c} - {d} - {e} - ");
			_ = builder.ToString();
			return builder;
		}
	}
}
