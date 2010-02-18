// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;
using System.IO;
using System.Text;
using System.Collections;

namespace HtmlAgilityPack
{
	/// <summary>
	/// Represents a document with mixed code and text. ASP, ASPX, JSP, are good example of such documents.
	/// </summary>
	public class MixedCodeDocument
	{
		private System.Text.Encoding _streamencoding = null;
		internal string _text;
		internal MixedCodeDocumentFragmentList _fragments;
		internal MixedCodeDocumentFragmentList _codefragments;
		internal MixedCodeDocumentFragmentList _textfragments;
		private ParseState _state;
		private int _index;
		private int _c;
		private int _line;
		private int _lineposition;
		private MixedCodeDocumentFragment _currentfragment;

		/// <summary>
		/// Gets or sets the token representing code start.
		/// </summary>
		public string TokenCodeStart = "<%";

		/// <summary>
		/// Gets or sets the token representing code end.
		/// </summary>
		public string TokenCodeEnd = "%>";

		/// <summary>
		/// Gets or sets the token representing code directive.
		/// </summary>
		public string TokenDirective = "@";

		/// <summary>
		/// Gets or sets the token representing response write directive.
		/// </summary>
		public string TokenResponseWrite = "Response.Write ";


		private string TokenTextBlock = "TextBlock({0})";

		/// <summary>
		/// Creates a mixed code document instance.
		/// </summary>
		public MixedCodeDocument()
		{
			_codefragments = new MixedCodeDocumentFragmentList(this);
			_textfragments = new MixedCodeDocumentFragmentList(this);
			_fragments = new MixedCodeDocumentFragmentList(this);
		}

		/// <summary>
		/// Loads a mixed code document from a stream.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		public void Load(Stream stream)
		{
			Load(new StreamReader(stream));
		}

		/// <summary>
		/// Loads a mixed code document from a stream.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
		{
			Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
		}

		/// <summary>
		/// Loads a mixed code document from a stream.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		public void Load(Stream stream, Encoding encoding)
		{
			Load(new StreamReader(stream, encoding));
		}

		/// <summary>
		/// Loads a mixed code document from a stream.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
		}

		/// <summary>
		/// Loads a mixed code document from a stream.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <param name="buffersize">The minimum buffer size.</param>
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}

		/// <summary>
		/// Loads a mixed code document from a file.
		/// </summary>
		/// <param name="path">The complete file path to be read.</param>
		public void Load(string path)
		{
			Load(new StreamReader(path));
		}

		/// <summary>
		/// Loads a mixed code document from a file.
		/// </summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		public void Load(string path, bool detectEncodingFromByteOrderMarks)
		{
			Load(new StreamReader(path, detectEncodingFromByteOrderMarks));
		}

		/// <summary>
		/// Loads a mixed code document from a file.
		/// </summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		public void Load(string path, Encoding encoding)
		{
			Load(new StreamReader(path, encoding));
		}

		/// <summary>
		/// Loads a mixed code document from a file.
		/// </summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks));
		}

		/// <summary>
		/// Loads a mixed code document from a file.
		/// </summary>
		/// <param name="path">The complete file path to be read.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
		/// <param name="buffersize">The minimum buffer size.</param>
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}

		/// <summary>
		/// Loads a mixed document from a text
		/// </summary>
		/// <param name="html">The text to load.</param>
		public void LoadHtml(string html)
		{
			Load(new StringReader(html));
		}

		/// <summary>
		/// Loads the mixed code document from the specified TextReader.
		/// </summary>
		/// <param name="reader">The TextReader used to feed the HTML data into the document.</param>
		public void Load(TextReader reader)
		{
			_codefragments.Clear();
			_textfragments.Clear();

			// all pseudo constructors get down to this one
			StreamReader sr = reader as StreamReader;
			if (sr != null)
			{
				_streamencoding = sr.CurrentEncoding;
			}

			_text = reader.ReadToEnd();
			reader.Close();
			Parse();
		}

		internal System.Text.Encoding GetOutEncoding()
		{
			if (_streamencoding != null)
				return _streamencoding;
			return System.Text.Encoding.Default;
		}

		/// <summary>
		/// Gets the encoding of the stream used to read the document.
		/// </summary>
		public System.Text.Encoding StreamEncoding
		{
			get
			{
				return _streamencoding;
			}
		}

		/// <summary>
		/// Gets the list of code fragments in the document.
		/// </summary>
		public MixedCodeDocumentFragmentList CodeFragments
		{
			get
			{
				return _codefragments;
			}
		}

		/// <summary>
		/// Gets the list of text fragments in the document.
		/// </summary>
		public MixedCodeDocumentFragmentList TextFragments
		{
			get
			{
				return _textfragments;
			}
		}

		/// <summary>
		/// Gets the list of all fragments in the document.
		/// </summary>
		public MixedCodeDocumentFragmentList Fragments
		{
			get
			{
				return _fragments;
			}
		}

		/// <summary>
		/// Saves the mixed document to the specified stream.
		/// </summary>
		/// <param name="outStream">The stream to which you want to save.</param>
		public void Save(Stream outStream)
		{
			StreamWriter sw = new StreamWriter(outStream, GetOutEncoding());
			Save(sw);
		}

		/// <summary>
		/// Saves the mixed document to the specified stream.
		/// </summary>
		/// <param name="outStream">The stream to which you want to save.</param>
		/// <param name="encoding">The character encoding to use.</param>
		public void Save(Stream outStream, System.Text.Encoding encoding)
		{
			StreamWriter sw = new StreamWriter(outStream, encoding);
			Save(sw);
		}

		/// <summary>
		/// Saves the mixed document to the specified file.
		/// </summary>
		/// <param name="filename">The location of the file where you want to save the document.</param>
		public void Save(string filename)
		{
			StreamWriter sw = new StreamWriter(filename, false, GetOutEncoding());
			Save(sw);
		}

		/// <summary>
		/// Saves the mixed document to the specified file.
		/// </summary>
		/// <param name="filename">The location of the file where you want to save the document.</param>
		/// <param name="encoding">The character encoding to use.</param>
		public void Save(string filename, System.Text.Encoding encoding)
		{
			StreamWriter sw = new StreamWriter(filename, false, encoding);
			Save(sw);
		}

		/// <summary>
		/// Saves the mixed document to the specified StreamWriter.
		/// </summary>
		/// <param name="writer">The StreamWriter to which you want to save.</param>
		public void Save(StreamWriter writer)
		{
			Save((TextWriter)writer);
		}

		/// <summary>
		/// Saves the mixed document to the specified TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter to which you want to save.</param>
		public void Save(TextWriter writer)
		{

			writer.Flush();
		}

		/// <summary>
		/// Gets the code represented by the mixed code document seen as a template.
		/// </summary>
		public string Code
		{
			get
			{
				string s = "";
				int i = 0;
				foreach(MixedCodeDocumentFragment frag in _fragments)
				{
					switch(frag._type)
					{
						case MixedCodeDocumentFragmentType.Text:
							s += TokenResponseWrite + string.Format(TokenTextBlock, i) + "\n";
							i++;
							break;

						case MixedCodeDocumentFragmentType.Code:
							s += ((MixedCodeDocumentCodeFragment)frag).Code + "\n";
							break;
					}
				}
				return s;
			}
		}

		/// <summary>
		/// Create a text fragment instances.
		/// </summary>
		/// <returns>The newly created text fragment instance.</returns>
		public MixedCodeDocumentTextFragment CreateTextFragment()
		{
			return (MixedCodeDocumentTextFragment)CreateFragment(MixedCodeDocumentFragmentType.Text);
		}

		/// <summary>
		/// Create a code fragment instances.
		/// </summary>
		/// <returns>The newly created code fragment instance.</returns>
		public MixedCodeDocumentCodeFragment CreateCodeFragment()
		{
			return (MixedCodeDocumentCodeFragment)CreateFragment(MixedCodeDocumentFragmentType.Code);
		}

		internal MixedCodeDocumentFragment CreateFragment(MixedCodeDocumentFragmentType type)
		{
			switch(type)
			{
				case MixedCodeDocumentFragmentType.Text:
					return new MixedCodeDocumentTextFragment(this);

				case MixedCodeDocumentFragmentType.Code:
					return new MixedCodeDocumentCodeFragment(this);

				default:
					throw new NotSupportedException();
			}
		}

		private void SetPosition()
		{
			_currentfragment._line = _line;
			_currentfragment._lineposition = _lineposition;
			_currentfragment._index = _index - 1;
			_currentfragment._length = 0;
		}

		private void IncrementPosition()
		{
			_index++;
			if (_c == 10)
			{
				_lineposition = 1;
				_line++;
			}
			else
				_lineposition++;
		}

		private enum ParseState
		{
			Text,
			Code
		}

		private void Parse()
		{
			_state = ParseState.Text;
			_index = 0;
			_currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Text);

			while (_index<_text.Length)
			{
				_c = _text[_index];
				IncrementPosition();

				switch(_state)
				{
					case ParseState.Text:
						if (_index+TokenCodeStart.Length<_text.Length)
						{
							if (_text.Substring(_index-1, TokenCodeStart.Length) == TokenCodeStart)
							{
								_state = ParseState.Code;
								_currentfragment._length = _index -1 - _currentfragment._index;
								_currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Code);
								SetPosition();
								continue;
							}
						}
						break;

					case ParseState.Code:
						if (_index+TokenCodeEnd.Length<_text.Length)
						{
							if (_text.Substring(_index-1, TokenCodeEnd.Length) == TokenCodeEnd)
							{
								_state = ParseState.Text;
								_currentfragment._length = _index + TokenCodeEnd.Length - _currentfragment._index;
								_index += TokenCodeEnd.Length;
								_lineposition += TokenCodeEnd.Length;
								_currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Text);
								SetPosition();
								continue;
							}
						}
						break;
				}
			}

			_currentfragment._length = _index - _currentfragment._index;
		}

	}
}
