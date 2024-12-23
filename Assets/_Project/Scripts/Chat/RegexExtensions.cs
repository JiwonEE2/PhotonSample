using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// static : 확장 메서드 만들기 위해 선언
public static class RegexExtensions
{
	// Regex : Regular Expression (정규 표현식)

	// 닉네임으로 0~9, a~z, A~Z, 가~힣 안에 포함되는 완성형 한글과 영문/숫자만 포함하는 정규식
	// ^ : not 연산자
	private static Regex nicknameRegex = new Regex(@"[^0-9A-Za-z가-힣]");

	// Regex 사용하는 방법
	// 1. Regex 클래스 사용
	// this : 확장 메서드 사용을 위해 선언
	public static bool NicknameValidate(this string nickname)
	{
		return false == nicknameRegex.IsMatch(nickname);
	}

	// 2. Regex 포맷 문자열로 변환
	// 문자열 입력 중에 미완성형 한글을 허용하는 정규식
	const string INPUT_FORM = @"[^0-9A-Za-z가-힣ㄱ-ㅎㅏ-ㅣㆍᆞᆢ]";

	public static string ToValidString(this string param)
	{
		return Regex.Replace(param, INPUT_FORM, "", RegexOptions.Singleline);
	}

	// 비속어 차단
	private const string COMPLETE_HANGEUL = @"[^가-힣]";

	// 일반 비속어
	private static List<string> fword = new()
	{
		"나쁜말", "욕", "바보", "멍청이"
	};

	// 변형 비속어
	private static List<string> irregularFword = new()
	{
		"ㅅH771", "પ ન\u0ac1લ\u0ac1\u0a82ગ લસશ"
	};

	public static bool ContainsFword(this string param)
	{
		if (string.IsNullOrEmpty(param)) return false;

		// 변형 비속어를 먼저 검사
		if (irregularFword.Exists(x => param.Contains(x))) return true;

		// 완성형 한글만 남김 예 : 나쁜1말 => 나쁜말
		param = Regex.Replace(param, COMPLETE_HANGEUL, "", RegexOptions.Singleline);
		return fword.Exists(x => param.Contains(x));
	}
}