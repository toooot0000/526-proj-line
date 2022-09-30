using System;
using Model;
using UI.Common;
using UnityEngine;

namespace UI{
    public class CoinDisplayer: MonoBehaviour{
        public CoinWithNumber coin;

        private void Start(){
            GameManager.shared.game.player.OnCoinChanged += (game, model) => coin.Number = ((Player)model).Coin;
            coin.Number = GameManager.shared.game.player.Coin;
        }
    }
}