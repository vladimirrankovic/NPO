using System;

namespace RDotNet.Internals
{
	internal interface INativeMethodsProxy
	{
		int Rf_initEmbeddedR(int argc, string[] argv);
		void Rf_endEmbeddedR(int fatal);
		IntPtr Rf_protect(IntPtr sexp);
		void Rf_unprotect(int count);
		void Rf_unprotect_ptr(IntPtr sexp);
		IntPtr Rf_install(string s);
		IntPtr Rf_mkString(string s);
		IntPtr Rf_mkChar(string s);
		IntPtr Rf_allocVector(SymbolicExpressionType type, int length);
		IntPtr Rf_coerceVector(IntPtr sexp, SymbolicExpressionType type);
		bool Rf_isVector(IntPtr sexp);
		int Rf_length(IntPtr sexp);
		IntPtr Rf_allocMatrix(SymbolicExpressionType type, int rowCount, int columnCount);
		bool Rf_isMatrix(IntPtr sexp);
		int Rf_nrows(IntPtr sexp);
		int Rf_ncols(IntPtr sexp);
		IntPtr Rf_allocList(int length);
		bool Rf_isList(IntPtr sexp);
		IntPtr Rf_eval(IntPtr statement, IntPtr environment);
		IntPtr R_tryEval(IntPtr statement, IntPtr environment, out bool errorOccurred);
		IntPtr R_ParseVector(IntPtr statement, int statementCount, out ParseStatus status, IntPtr _);
		IntPtr Rf_findVar(IntPtr name, IntPtr environment);
		void Rf_setVar(IntPtr name, IntPtr value, IntPtr environment);
		IntPtr Rf_getAttrib(IntPtr sexp, IntPtr name);
		IntPtr Rf_setAttrib(IntPtr sexp, IntPtr name, IntPtr value);
		bool Rf_isEnvironment(IntPtr sexp);
		bool Rf_isExpression(IntPtr sexp);
		bool Rf_isSymbol(IntPtr sexp);
		bool Rf_isLanguage(IntPtr sexp);
		bool Rf_isFunction(IntPtr sexp);
		IntPtr R_lsInternal(IntPtr environment, bool all);
		IntPtr Rf_applyClosure(IntPtr call, IntPtr value, IntPtr arguments, IntPtr environment, IntPtr suppliedEnvironment);
		IntPtr Rf_VectorToPairList(IntPtr sexp);
		IntPtr Rf_allocSExp(SymbolicExpressionType type);
		IntPtr Rf_cons(IntPtr sexp, IntPtr next);
		IntPtr Rf_lcons(IntPtr sexp, IntPtr next);
		void R_DefParams(out RStart start);
		void R_SetParams(ref RStart start);
		void R_set_command_line_arguments(int argc, string[] argv);
		void R_common_command_line(ref int argc, string[] argv, ref RStart start);
		void R_setStartTime();
		void setup_Rmainloop();
		int Rf_initialize_R(int ac, string[] argv);
	}
}
