/*
 * New Frontiers - This file is licensed under AGPLv3
 * Copyright (c) 2024 New Frontiers Contributors
 * See AGPLv3.txt for details.
 */
using Content.Client.UserInterface.Controls;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Bank.UI;

[GenerateTypedNameReferences]
public sealed partial class StationBankATMMenu : FancyWindow
{
    public Action? WithdrawRequest;
    public Action? DepositRequest;
    public int Amount;
    private readonly List<string> _reasonStrings = new();
    public string? Reason;
    public string? Description;
    public StationBankATMMenu()
    {
        RobustXamlLoader.Load(this);
        WithdrawButton.OnPressed += OnWithdrawPressed;
        DepositButton.OnPressed += OnDepositPressed;
        Title = Loc.GetString("station-bank-atm-menu-title");
        WithdrawEdit.OnTextChanged += OnAmountChanged;
        Reasons.OnItemSelected += OnReasonSelected;
        AmountDescription.OnTextChanged += OnDescChanged;
    }

    private void SetReasonText(int id)
    {
        Reason = id == 0 ? null : _reasonStrings[id];
        Reasons.SelectId(id);
    }
    private void OnReasonSelected(OptionButton.ItemSelectedEventArgs args)
    {
        SetReasonText(args.Id);
    }
    public void SetBalance(int amount)
    {
        BalanceLabel.Text = Loc.GetString("bank-atm-menu-cash-amount", ("amount", amount.ToString()));
    }

    public void SetDeposit(int amount)
    {
        DepositButton.Disabled = amount <= 0;
        if (amount >= 0) // Valid
            DepositLabel.Text = Loc.GetString("bank-atm-menu-cash-amount", ("amount", amount.ToString()));
        else
            DepositLabel.Text = Loc.GetString("bank-atm-menu-cash-error");
    }

    public void SetEnabled(bool enabled)
    {
        WithdrawButton.Disabled = !enabled;
        DepositButton.Disabled = !enabled;
    }

    private void OnWithdrawPressed(BaseButton.ButtonEventArgs obj)
    {
        WithdrawRequest?.Invoke();
    }

    private void OnDepositPressed(BaseButton.ButtonEventArgs obj)
    {
        DepositRequest?.Invoke();
    }

    private void OnAmountChanged(LineEdit.LineEditEventArgs args)
    {
        if (int.TryParse(args.Text, out var amount))
        {
            Amount = amount;
        }    
    }

    private void OnDescChanged(LineEdit.LineEditEventArgs args)
    {
        Description = args.Text;
    }

    public void PopulateReasons()
    {
        _reasonStrings.Clear();
        Reasons.Clear();
        //todo: think of a better way/place to store the petty cash reason strings. this is mostly for rp, and a little bit of admin qol
        _reasonStrings.Add("default");
        _reasonStrings.Add("payroll");
        _reasonStrings.Add("workorder");
        _reasonStrings.Add("supplies");
        _reasonStrings.Add("bounty");
        _reasonStrings.Add("other");
        Reasons.AddItem(Loc.GetString("station-bank-required"), 0);
        Reasons.AddItem(Loc.GetString("station-bank-payroll"), 1);
        Reasons.AddItem(Loc.GetString("station-bank-workorder"), 2);
        Reasons.AddItem(Loc.GetString("station-bank-supplies"), 3);
        Reasons.AddItem(Loc.GetString("station-bank-bounty"), 4);
        Reasons.AddItem(Loc.GetString("station-bank-other"), 5);
    }
}
