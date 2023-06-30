using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Increlemental;

public class Currency
{
	public double Value { get; set; } = 0;
	public uint Exp { get; set; } = 1;

	public string[] Magnitudes = { "", "K", "M", "B", "T" };

	public Currency() { }

	public Currency( double value )
	{
		Value = value;
		Normalize();
	}

	public Currency( double value, uint exp )
	{
		Value = value;
		Exp = exp;
		Normalize();
	}

	public void Normalize()
	{
		while ( Math.Abs( Value ) >= Math.Pow( 10, 3 ) )
		{
			Value /= Math.Pow( 10, 3 );
			Exp += 3;
		}
	}

	public Currency AlignTo( uint exp )
	{
		uint diff = exp - Exp;
		double value = Value / Math.Pow( 10, diff );

		return new Currency( value, exp );
	}

	public static Currency operator +( Currency a, Currency b )
	{
		double value;
		Currency result;

		if ( a.Exp >= b.Exp )
		{
			value = b.AlignTo( a.Exp ).Value + a.Value;
			result = new Currency( value, a.Exp );
		}
		else
		{
			value = a.AlignTo( b.Exp ).Value + b.Value;
			result = new Currency( value, b.Exp );
		}

		result.Normalize();
		return result;
	}

	public static Currency operator +( Currency a, int b )
	{
		Currency bCurrency = new( b, 1 );
		Currency result = new Currency( a.Value + bCurrency.AlignTo( a.Exp ).Value, a.Exp );

		result.Normalize();
		return result;
	}

	public static Currency operator -( Currency a, Currency b )
	{
		double value;
		Currency result;

		if ( a.Exp >= b.Exp )
		{
			value = a.Value - b.AlignTo( a.Exp ).Value;
			result = new Currency( value, a.Exp );
		}
		else
		{
			value = b.Value - a.AlignTo( b.Exp ).Value;
			result = new Currency( value, b.Exp );
		}

		result.Normalize();
		return result;
	}

	public string GetUnit()
	{
		string unit;
		int charA = Convert.ToInt32( 'a' );
		uint magnitude = Exp / 3;

		if (magnitude < Magnitudes.Length)
			unit = Magnitudes[ magnitude ];
		else
		{
			var unitInt = magnitude - Magnitudes.Length;
			var secondUnit = unitInt % 26;
			var firstUnit = unitInt / 26;
			unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
		}

		return unit;
	}

	public override string ToString()
	{
		return Value.ToString( "0.##" ) + GetUnit();
	}
}
