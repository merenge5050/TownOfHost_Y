using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace TownOfHostY;

public class ModNews
{
    public int Number;
    public int BeforeNumber;
    public string Title;
    public string SubTitle;
    public string ShortTitle;
    public string Text;
    public string Date;

    public Announcement ToAnnouncement()
    {
        var result = new Announcement
        {
            Number = Number,
            Title = Title,
            SubTitle = SubTitle,
            ShortTitle = ShortTitle,
            Text = Text,
            Language = (uint)DataManager.Settings.Language.CurrentLanguage,
            Date = Date,
            Id = "ModNews"
        };

        return result;
    }
}
[HarmonyPatch]
public class ModNewsHistory
{
    public static List<ModNews> AllModNews = new();

    public static void Init()
    {
        {
            var news = new ModNews
            {
                Number = 100002,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v3.0.2.3",
                SubTitle = "★★★★v3.0.2.3アップデート!★★★★",
                ShortTitle = "★TOH_Y v3.0.2.3",
                Text = "TownOfHost_Yのご利用ありがとうございます！\n"

                    + "\n 【新役職】呪狼・マッドシェリフ・バカシェリフ・共鳴者・ダークハイド・ラブカッター・オポチュニストキラー(オポチュニストのキル可能設定)"
                    + "\n 【新レイアウトの設定画面】：陣営ごとのタブ\n"
                    + "\n ホストが重くカク付く現象を比較的修正。さらに負荷軽減設定(ワカホリ,にじスタ対象)を追加。全員から役職名見えるのが重いの..etc.\n"

                    + "\n【注意】\nプレイする際は必ずTOH_Yであることを明記・通知してください。本家TOHではないことを十分理解した上で(参加者にも理解させた上で)ご利用ください。"
                    + "\nまた、本家TOHとTOH_Yの同時使用はできません。必ず1つのMODだけを使用するようにしてください。",
                Date = "2022-11-21T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100003,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v402.3.9",
                SubTitle = "★★★★v402.3.9アップデート!★★★★",
                ShortTitle = "★TOH_Y v402.3.9",
                Text = "Thank you for playing TownOfHost_Y!\n"

                    + "\n 【対応】AU本体v2022.12.8・本家TOHv4.0.2"
                    + "\n 【新役職】グリーディア・アンビショナー・ブラインダー・純愛者"
                    + "\n 【新属性】ウォッチング・ライティング・サングラス・シーイング"
                    + "\n 【(予告)新要素】新ゲームモード『猫取合戦』"
                    + "\n ここはそれぞれ孤独な猫たちが集う場。いつも自身のタスクをこなしながら穏やかな日々を過ごしていた。或る日、この地の神がここの孤独な猫たちを見て、裏切り者が生まれた時に備えていくつか集団を作るべきと判断し、数名にリーダーを任命した。\n"

                    + "\n【注意】プレイの際は必ずTOH_Yであることを通知して下さい。",
                Date = "2023-1-12T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100004,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v402.4",
                SubTitle = "★★★★★v402.4アップデート!★★★★★",
                ShortTitle = "★TOH_Y v402.4",
                Text = "Thank you for playing TownOfHost_Y!\n"
                    + "\n 【新要素】新ゲームモード『猫取合戦』"
                    + "\n ここはそれぞれ孤独な猫たちが集う場。いつも自身のタスクをこなしながら穏やかな日々を過ごしていた。或る日、この地の神がここの孤独な猫たちを見て、裏切り者が生まれた時に備えていくつか集団を作るべきと判断し、数名にリーダーを任命した。\n"

                    + "\n【注意】プレイの際は必ずTOH_Yであることを通知して下さい。本家TOHではないことを十分理解した上で(参加者にも理解させた上で)ご利用ください。"
                    + "特に公開部屋で遊ぶときは注意してください。自分の名前の上にTownOfHost_Yと表記されたり、参加者に自動的にお知らせが流れたりしますが、「TOHだよ！」と偽ったり「TOHの新バージョン」のように本家と区別がつかない言い方をするのはお辞めください。\n"
                    + "\nまた、本家TOHとTOH_Yの同時使用はできません。必ず1つのMODだけを使用するようにしてください。",
                Date = "2023-1-15T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100005,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v402.5",
                SubTitle = "★★★★★v402.5アップデート!★★★★★",
                ShortTitle = "★TOH_Y v402.5",
                Text = "Thank you for playing TownOfHost_Y!\n"

                    + "\n 【新役職】弁護士(追跡者)・[属性]オートプシー"
                    + "\n 【新機能】シンクロカラーモード"
                    + "\n 　　　　-----クローン・50-50・三つ巴・ツイン"
                    + "\n スキンは勿論のこと、ペット、色、ネームプレート、プレイヤー、レベルまで何から何まで一緒なため、自分が会議の何番目にいるのかすらわからない時もある圧倒的お遊び系モード。\n"

                    + "\n【注意】プレイの際は必ずTOH_Yであることを通知して下さい。"
                    + "\n・野良でのwelcomeメッセージやYouTube、Twitter等を見ているとTOH_Yを使用しているにも関わらず、TownOfHost(無印)になっているのをよく見かけます。"
                    + "本家TOHと勘違いされるパターンも実際起こりますので充分お気を付けください。"
                    + "\n・特に公開ルームで部屋を立てる際も、何もわからずに参加してくる方にでも本家TOHでないことが伝わるような通知を心がけてください。\n何卒宜しくお願い致します。 ",
                Date = "2023-1-26T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100006,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v402.6",
                SubTitle = "★★★★★v402.6アップデート!★★★★★",
                ShortTitle = "★TOH_Y v402.6",
                Text = "Thank you for playing TownOfHost_Y!\n"

                    + "\n 【新役職】クライアント"
                    + "\n 【新属性】VIP・クラムシー・リベンジャー"
                    + "\n 【新機能】既存役職の新オプションを12種追加"
                    + "\n 【新機能】「自身の役職説明自動表示」を含む新機能を5種追加"
                    + "\n 【期間限定役職】チョコレート屋～2/15\n"

                    + "\n【注意】プレイの際は必ずTOH_Yであることを通知して下さい。"
                    + "\n・野良でのwelcomeメッセージやYouTube、Twitter等を見ているとTOH_Yを使用しているにも関わらず、TownOfHost(無印)になっているのをよく見かけます。"
                    + "本家TOHと勘違いされるパターンも実際起こりますので充分お気を付けください。"
                    + "\n・特に公開ルームで部屋を立てる際も、何もわからずに参加してくる方にでも本家TOHでないことが伝わるような通知を心がけてください。\n何卒宜しくお願い致します。 ",
                Date = "2023-02-09T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100007,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v411.7",
                SubTitle = "★★★★★v411.7アップデート!★★★★★",
                ShortTitle = "★TOH_Y v411.7",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>新MODゲームモード【ワンナイト】実装！</b>\n"
                + "\n　一回の議論だけで人狼を探し出す「ワンナイト人狼」をAmong Usでアレンジして実装！一回の行動ターンと一回の会議で人狼を見つけて追放しましょう。"
                + "\n　行動ターンの要素と、役職を基とした議論の要素と。どの視点で詰めていくかはお任せします。。。\n"
                + "\n　詳しい説明は[/h n]コマンド、Discord、GitHub何れかから！\n"
                + "\n<b>-【ワンナイトモードの役職】</b>"
                + "\n　・人狼/大狼"
                + "\n　・狂人/狂信者"
                + "\n　・村人/占い師/怪盗/村長/狩人/パン屋/罠師"
                + "\n　・吊人\n"
                + "\n----------------------------------------------------------------"
                + "\n　スタンダードモードもいくつかアップデート。\n"
                + "\n<b>-【新役職】スカベンジャー</b>"
                + "\n　スカベンジャーにキルされた死体は、通報することが出来なくなる。(通報ボタンを押しても反応しない)\n"
                + "\n<b>-【新属性】マネジメント</b>"
                + "\n　タスクマネージャーの属性版。全員の完了タスク合計数がわかる。(デフォルトは会議中のみ)\n"
                + "\n<b>-【新機能】にじいろスター</b>"
                + "\n　ネームプレートが虹色になるようになりました。また設定で、タスクターン中、他視点から役職名が表示されない設定を追加しました。\n"
                + "\n<b>-【期間限定役職復刻】チョコレート屋</b> ～3/15"
                + "\n　ホワイトデーでお返しを！チョコレート屋が帰ってきます。コメントを忘れずに確認してね！\n"
                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-03-09T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100008,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v412.8",
                SubTitle = "★★★★★v412.8アップデート!★★★★★",
                ShortTitle = "★TOH_Y v412.8",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>エイプリルフールだ！Yで遊ばない？</b>\n"
                + "\n　ポテンシャリストという意味の分からない役職が登場！"
                + "\n　タスクを完了させるとランダムなクルー役職に変化、能力が開花しちゃいます。残念ながら期間限定なので今後また遊べるかはわかりません。。。"
                + "\n----------------------------------------------------------------"
                + "\n　その他アップデート内容↓↓↓\n"
                + "\n<b>-【新役職】イビルディバイナー</b>"
                + "\n　いわゆる占い師の能力が使えるインポスター。キルボタンを1回だけ押すと占うことができるけど、キルクールは消費してしまうので占うかキルか、迷いながら行動しよう。\n"
                + "\n<b>-【新役職】テレパシスターズ</b>"
                + "\n　イビルトラッカーの複数人版を、Ｙでアレンジしてみました。以心伝心でキルしていこう。新要素として、シスターズ同士のベント回数が共有されていたりします。\n"
                + "\n<b>-【新役職】メディック</b>"
                + "\n　ベントでプレイヤーを選択するという新たな機能が登場。このメディックは、選んだプレイヤーを一度キルガードします。\n"
                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-03-31T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100009,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v412.9",
                SubTitle = "★★★★★v412.9アップデート!★★★★★",
                ShortTitle = "★TOH_Y v412.9",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>なんと、なんと、新属性大量発生！</b>\n"
                + "\n　マッド系役職でよく使われている(らしい)、属性が一斉に新登場！"
                + "\n　ランダムで属性を付与する【ADD-ON Setting】以外にも、<b>ラストインポスター</b>や、今回の新登場属性<b>コンプリートクルー</b>にも属性を直接付与することができるのでぜひお試しあれ。"
                + "\n----------------------------------------------------------------"
                + "\n<b>-【新役職】シェイプキラー・グラージシェリフ・キャンドルライター・占い師・霊媒師・トトカルチョ</b>"
                + "\n　クライアントの制作者、くろにゃんこ氏と再びコラボレーション！シェイプキラーと占い師はくろにゃんこ氏に開発していただきました！\n"
                + "\n<b>-【新属性】インフォプアー・タイブレーカー・ノンレポート・センディング・ロイヤルティ・プラスボート・ガーディング・ベイティング・リフュージング・コンプリートクルー</b>"
                + "\n　より能力の幅を広くするため、新属性の開発を行いました。マッドメイトをさらに強くするもよし、ランダムにデバフ属性のみを付与させて遊ぶもよし、色々な遊び方を試してみてください。\n"
                + "\n<b>-【新機能】役職設定モード</b>（スタンダードモードのみ）"
                + "\n　0%100%のみの設定方法から、オンオフの設定に切り替えました。本家TOH対応に向けての機能です。また、属性のみしか設定できない属性オンリーモードも作ってみました。議論がなかなか面白くなりそうな予感がします。\n"
                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-05-5T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100010,
                //BeforeNumber = 0,
                Title = "Town Of Host_YS v501.10.2",
                SubTitle = "★★★★★v501.10.2アップデート★★★★★",
                ShortTitle = "★TOH_YS v501.10.2",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>対応までしばらくは制限版で、、、</b>\n"
                + "\n　Among Usバニラ側の内部的仕様変更により、ホスト系MOD全てでキル関連に関する問題が発生しています。"
                + "\n　それに伴い、以下役職が正常に使用できません。(キルがそのまま通る・部屋からBANされる等)"
                + "\n----------------------------------------------------------------"
                + "\n<b>-【インポスター】</b>パペッティア・ウィッチ・ヴァンパイア・ウォーロック・イビルディバイナー・呪狼</b>"
                + "\n<b>-【マッド】</b>マッドガーディアン・マッドシェリフ</b>"
                + "\n<b>-【クルー】</b>シェリフ/バカシェリフ・グラージシェリフ</b>"
                + "\n<b>-【第三】</b>アーソニスト・シュレディンガーの猫・純愛者・トトカルチョ・ラブカッター・ダークハイド</b>"
                + "\n<b>-【属性】</b>ガーディング</b>"
                + "\n<b>-【一部機能不可】</b>アンチコンプリートガード・追跡者ガード</b>"
                + "\n\n　また、TOH_YS未実装は上記と別に以下の通りです。"
                + "\n----------------------------------------------------------------"
                + "\n<b>-【インポスター】</b>シェイプキラー"
                + "\n<b>-【クルー】</b>メディック・霊媒師"
                + "\n<b>-【第三】</b>クライアント・弁護士"
                + "\n<b>-【属性】</b>Yで追加したもの全て"
                + "\n<b>-【機能未実装】</b>マッドメイト/ラストインポスターの属性直接付与設定\n"

                + "\n　対応までご不便をおかけしますが、今後ともTOH_Yをよろしくお願いいたします。"
                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-06-22T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100011,
                //BeforeNumber = 0,
                Title = "Town Of Host_YS おススメ役職【ハンター】",
                SubTitle = "シェリフが使えないから、代わりにどうだい？",
                ShortTitle = "★ハンターをおススメします",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>シェリフは使えません</b>"
                + "\n　シェリフが今回の未対応役職の1つになってしまったのはかなりの痛手です。\n"
                + "\n　そこで、前からTOH_Yに実装されている【ハンター】をおススメさせていただきます。"
                + "\n----------------------------------------------------------------"
                + "\n<b>ハンター</b>"
                + "\n　誰に向かってでもキルできる。設定回数キル可能。"
                + "\n　あくまでクルー陣営なのでタイミングは考えよう。タスクはもっていない。"
                + "\n　「キルした時敵陣営かどうかわかる」設定がオンの時、"
                + "\n　インポスターをキルした時、自身の名前に◎が付く。第三陣営をキルした時、自身の名前に▽が付く。"
                + "\n　(マッドメイトはインポスターと同じ◎にするか、クルーと同じ無印にするか設定可能。)"
                + "\n----------------------------------------------------------------"
                + "\nいつもシェリフを使っている人に説明するとすると、"
                + "\n【クルーをキルする時、自身が自爆するのではなく<b>ただ相手をキルしてしまう</b>】という感じ。"
                + "\n　シェリフが使えない今だからこそ、このハンターを使用してみてください！\n"
                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-06-22T00:00:01Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100012,
                //BeforeNumber = 0,
                Title = "Town Of Host_YS v502.12",
                SubTitle = "★★★★★v502.12アップデート★★★★★",
                ShortTitle = "★TOH_YS v502.12",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>ごめんね、まだシェリフは使えないんだ。</b>\n"
                + "\n　なにげにちょこっと新役職追加。インポスターも用意していたけど鯖変更のやつにひっかかっちゃった"
                + "\n<b>-【新役職】マッドニムロッド・ニムロッド・決闘者</b>"
                + "\n　ニムロッドはワンナイトモードの狩人と能力は同じです。道連れしたい人を指名することができます。"
                + "\n　決闘者は第三陣営の追加勝利役職。好きな人を宿敵に選んでその人とバチバチ戦おう。\n"
                + "\n----------------------------------------------------------------"
                + "\n　Among Usバニラ側の内部的仕様変更により、ホスト系MOD全てでキル関連に関する問題が発生しています。"
                + "\n　それに伴い、以下役職が正常に使用できません。(既に設定できないように隠してあります。)"
                + "\n----------------------------------------------------------------"
                + "\n<b>-【インポスター】</b>パペッティア・ウィッチ・ヴァンパイア・ウォーロック・イビルディバイナー・呪狼</b>"
                + "\n<b>-【マッド】</b>マッドガーディアン・マッドシェリフ</b>"
                + "\n<b>-【クルー】</b>シェリフ/バカシェリフ</b>"
                + "\n<b>-【第三】</b>アーソニスト・シュレディンガーの猫・純愛者・トトカルチョ・ラブカッター</b>"
                + "\n<b>-【属性】</b>ガーディング</b>"
                + "\n<b>-【一部機能不可】</b>アンチコンプリートガード・追跡者ガード</b>"

                + "\n　対応までご不便をおかけしますが、今後ともTOH_Yをよろしくお願いいたします。"
                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-07-12T00:00:00Z"

            };
            AllModNews.Add(news);
        }
        {
            var news = new ModNews
            {
                Number = 100013,
                //BeforeNumber = 0,
                Title = "Town Of Host_Y v503.13",
                SubTitle = "★★★★★v503.13アップデート★★★★★",
                ShortTitle = "★TOH_Y v503.13",
                Text = "Thank you for playing TownOfHost_Y!\n"
                + "\n<b>おかえり！ようやく全て使えるようになったよ！</b>\n"
                + "\n　シェリフ等一部役職が使用できなくなっていましたが、Innersloth側が提供してくれたホストMODのための仕様のおかげで"
                + "ようやく全ての役職が使用できるようになりました！これからも色んな役職で遊んでね。\n"
                + "\n----------------------------------------------------------------"
                + "\n<b>その代わり公開ルームが犠牲になるよ</b>\n"
                + "\n　ちゃんと動く状態のものは特殊仕様になっているため、公開ルームでの検索(野良)で他の人が入れなくなりました。"
                + "\nYでは設定(歯車)にて制限版にすることで公開ルームでも使用できるようにご用意いたしました。使えない役職は非表示にしているので、"
                + "表示されていて入れた役職で不具合が起こった時は報告してください。"
                + "\n----------------------------------------------------------------"
                + "\n<b>-【v12新役職】マッドニムロッド・ニムロッド・決闘者</b>"
                + "\n　ニムロッドはワンナイトモードの狩人と能力は同じです。道連れしたい人を指名することができます。"
                + "\n　決闘者は第三陣営の追加勝利役職。好きな人を宿敵に選んでその人とバチバチ戦おう。\n"

                + "\nなにか気になったことやバグ報告はTOH_YのDiscordまでご連絡ください。\n\nTown Of Host_Y：Yumeno",
                Date = "2023-07-14T00:00:00Z"

            };
            AllModNews.Add(news);
        }
    }

    [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements)), HarmonyPrefix]
    public static bool SetModAnnouncements(PlayerAnnouncementData __instance, [HarmonyArgument(0)] ref Il2CppReferenceArray<Announcement> aRange)
    {
        if (AllModNews.Count < 1)
        {
            Init();
            AllModNews.Sort((a1, a2) => { return DateTime.Compare(DateTime.Parse(a2.Date), DateTime.Parse(a1.Date)); });
        }

        List<Announcement> FinalAllNews = new();
        AllModNews.Do(n => FinalAllNews.Add(n.ToAnnouncement()));
        foreach (var news in aRange)
        {
            if (!AllModNews.Any(x => x.Number == news.Number))
                FinalAllNews.Add(news);
        }
        FinalAllNews.Sort((a1, a2) => { return DateTime.Compare(DateTime.Parse(a2.Date), DateTime.Parse(a1.Date)); });

        aRange = new(FinalAllNews.Count);
        for (int i = 0; i < FinalAllNews.Count; i++)
            aRange[i] = FinalAllNews[i];

        return true;
    }
}
