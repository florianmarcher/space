using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space.Scripts
{

	public class SpiralIterator : IEnumerator<Vector3Int>, IEnumerable<Vector3Int>
	{
		private enum SpiralState
		{
			Center,
			Bottom,
			Middle,
			Top
		}
		
		public Vector3Int Current { get; private set; }

		private int current_layer;
		private readonly int max_layer;
		private SpiralState state;

		public SpiralIterator(int layer_count)
		{
			max_layer = layer_count;
			Reset();
		}
		
		public bool MoveNext()
		{
			if (current_layer == -1)
			{
				current_layer = 0;
				return true;
			}
			
			switch (state)
			{
				case SpiralState.Center:
					return IncrementLayer();
				case SpiralState.Bottom:
					if (!NextInPlane())
					{
						state = SpiralState.Middle;
						Current = Vector3Int.one * -current_layer + new Vector3Int(0, 0, 1);
					}
					break;
				case SpiralState.Middle:
					if (!NextInMiddle())
					{
						if(Current.z == current_layer - 1)
						{
							state = SpiralState.Top;
							Current = new Vector3Int(-current_layer, -current_layer, current_layer);
						}
						else
						{
							Current = new Vector3Int(-current_layer, -current_layer, Current.z + 1);
						}
					}
					break;
				case SpiralState.Top:
					if (!NextInPlane())
						return IncrementLayer();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return true;
		}

		private bool IncrementLayer()
		{
			current_layer++;
			if (current_layer > max_layer)
				return false;
			state = SpiralState.Bottom;
			Current = Vector3Int.one * (current_layer * -1);
			return true;
		}

		private bool NextInPlane()
		{
			Current += Vector3Int.right;
			if(Current.x > current_layer)
				Current = new Vector3Int(-current_layer, Current.y + 1, Current.z);
			return Current.y <= current_layer;
		}

		private bool NextInMiddle()
		{
			if (Math.Abs(Current.y) - current_layer == 0)
				return NextInPlane();

			if (Current.x == current_layer)
				Current = new Vector3Int(-current_layer,Current.y + 1, Current.z);
			else
				Current = new Vector3Int(current_layer, Current.y, Current.z);
			return true;
		}

		public void Reset()
		{
			Current = Vector3Int.zero;
			current_layer = -1;
			state = SpiralState.Center;
		}


		object IEnumerator.Current => Current;

		public void Dispose()
		{
			Reset();
		}

		IEnumerator<Vector3Int> IEnumerable<Vector3Int>.GetEnumerator()
		{
			return this;
		}

		public override string ToString()
		{
			return Current + ", " + state + ", layer: " + current_layer;
		}

		public IEnumerator GetEnumerator()
		{
			return this;
		}
	}
}