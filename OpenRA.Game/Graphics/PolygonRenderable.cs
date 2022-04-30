using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRA.Primitives;
using SharpGLTF.Schema2;

namespace OpenRA.Graphics
{
	public class PolygonRenderable : IRenderable, IFinalizedRenderable
	{
		readonly WPos pos;
		readonly WVec offset;
		readonly int zOffset;
		readonly bool isDecoration;
		Vertex[] verticesArray;

		public PolygonRenderable(WPos pos, WVec offset, int zOffset, bool isDecoration)
		{
			this.pos = pos;
			this.offset = offset;
			this.zOffset = zOffset;
			this.isDecoration = isDecoration;


			var test = ModelRoot.Load(@"C:\Dev\other\openra\cubes.glb");

			var verticesList = new List<Vertex>();
			foreach (var cube in test.DefaultScene.VisualChildren)
			{

				var primitive = cube.Mesh.Primitives.First();
				var transform = cube.LocalTransform.Matrix;
				var matrix = new float[] {
				transform.M11,
				transform.M12,
				transform.M13,
				transform.M14,
				transform.M21,
				transform.M22,
				transform.M23,
				transform.M24,
				transform.M31,
				transform.M32,
				transform.M33,
				transform.M34,
				transform.M41,
				transform.M42,
				transform.M43,
				transform.M44,
			};

				var indices = primitive.IndexAccessor.AsIndicesArray();
				var vertices = primitive.GetVertexAccessor("POSITION").AsVector3Array();

				for (var i = 0; i < indices.Count; i++)
				{
					var index = (int)indices[i];
					var vertex = vertices[index];
					var vertexFloatArray = new float[] { vertex.X, vertex.Y, vertex.Z, 1 };

					vertexFloatArray = Util.MatrixVectorMultiply(matrix, vertexFloatArray);

					verticesList.Add(new Vertex(new float3(555 + vertexFloatArray[0] * 10, 525 + vertexFloatArray[1] * 10, vertexFloatArray[2] * 10), 1, 0, 0, 1, 0, 0));
				}
			}
			verticesArray = verticesList.ToArray();
		}

		public WPos Pos => pos + offset;
		public int ZOffset => zOffset;
		public bool IsDecoration => isDecoration;

		public IRenderable WithZOffset(int newOffset) { return new PolygonRenderable(pos, offset, newOffset, isDecoration); }
		public IRenderable OffsetBy(in WVec vec) { return new PolygonRenderable(pos + vec, offset, zOffset, isDecoration); }
		public IRenderable AsDecoration() { return new PolygonRenderable(pos, offset, zOffset, true); }

		public IFinalizedRenderable PrepareRender(WorldRenderer wr) { return this; }

		public void Render(WorldRenderer wr)
		{


			var color = Color.Red;
			var closed = true;

			var width = 1f;
			var blendMode = BlendMode.Alpha;
			var p = wr.Screen3DPxPosition(pos);
			var tl = (float3)wr.Viewport.WorldToViewPx(p);
			var br = (float3)wr.Viewport.WorldToViewPx(p + new float3(10, 10, 10));
			var tr = new float3(br.X, tl.Y, tl.Z);
			var bl = new float3(tl.X, br.Y, br.Z);
			var points = new[] { tl, tr, br, bl };

			// Not a line
			if (points.Length < 2)
				return;

			// Single segment
			if (points.Length == 2)
			{
				Game.Renderer.RgbaColorRenderer.DrawLine(points[0], points[1], width, color, blendMode);
				return;
			}

			color = Util.PremultiplyAlpha(color);
			var r = color.R / 255.0f;
			var g = color.G / 255.0f;
			var b = color.B / 255.0f;
			var a = color.A / 255.0f;

			var start = points[0];
			var end = points[1];
			var dir = (end - start) / (end - start).XY.Length;
			var corner = width / 2 * new float3(-dir.Y, dir.X, dir.Z);

			// Corners for start of line segment
			var ca = start - corner;
			var cb = start + corner;

			// Vertices for the corners joining start-end to end-next
			var cc = end + corner;
			var cd = end - corner;

			var Offset = new float3(0.5f, 0.5f, 0f);



			//vertices[0] = new Vertex(ca + Offset, r, g, b, a, 0, 0);
			//vertices[1] = new Vertex(cb + Offset, r, g, b, a, 0, 0);
			//vertices[2] = new Vertex(cc + Offset, r, g, b, a, 0, 0);
			//vertices[3] = new Vertex(cc + Offset, r, g, b, a, 0, 0);
			//vertices[4] = new Vertex(cd + Offset, r, g, b, a, 0, 0);
			//vertices[5] = new Vertex(ca + Offset, r, g, b, a, 0, 0);
			Game.Renderer.SpriteRenderer.DrawRGBAVertices(verticesArray, blendMode);
		}

		public void RenderDebugGeometry(WorldRenderer wr)
		{
		}

		public Rectangle ScreenBounds(WorldRenderer wr)
		{
			return new Rectangle();
		}
	}
}
