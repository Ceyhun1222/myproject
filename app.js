/* SIMA Pay — Telegram Mini App
   Port of the SwiftUI prototype: same screens, same copy, same logic. */

const tg = window.Telegram && window.Telegram.WebApp ? window.Telegram.WebApp : null;
const $ = (sel) => document.querySelector(sel);

const STORAGE_KEY = "simapay.cards.v1";

const state = {
  cards: [],
  pending: null,
  scanning: false,
  scanToken: 0,
};

/* ---------------- formatters (CardFormatter.swift) ---------------- */

const digitsOf = (raw) => (raw || "").replace(/\D/g, "");

function formatNumber(raw) {
  const digits = digitsOf(raw).slice(0, 16);
  return digits.replace(/(\d{4})(?=\d)/g, "$1 ");
}

function formatExpiry(raw) {
  const digits = digitsOf(raw).slice(0, 4);
  if (digits.length <= 2) return digits;
  return digits.slice(0, 2) + "/" + digits.slice(2);
}

function sanitizeCVV(raw) {
  return digitsOf(raw).slice(0, 3);
}

const isValidNumber = (raw) => digitsOf(raw).length === 16;
const isValidExpiry = (raw) => {
  const digits = digitsOf(raw);
  if (digits.length !== 4) return false;
  const mm = parseInt(digits.slice(0, 2), 10);
  return mm >= 1 && mm <= 12;
};
const isValidCVV = (raw) => digitsOf(raw).length === 3;

function brandOf(raw) {
  const first = digitsOf(raw)[0];
  if (first === "4") return "VISA";
  if (first === "5") return "Mastercard";
  if (first === "3") return "AMEX";
  return "CARD";
}

function maskedNumber(raw) {
  const digits = digitsOf(raw);
  if (digits.length < 4) return raw;
  return "•••• •••• •••• " + digits.slice(-4);
}

/* ---------------- store ---------------- */

function loadCards() {
  try {
    const saved = JSON.parse(localStorage.getItem(STORAGE_KEY) || "[]");
    state.cards = Array.isArray(saved) ? saved : [];
  } catch {
    state.cards = [];
  }
}

function saveCards() {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(state.cards));
  } catch {
    /* storage full or blocked — cards stay in memory */
  }
}

/* ---------------- telegram helpers ---------------- */

function hapticImpact(style) {
  try { tg && tg.HapticFeedback.impactOccurred(style || "medium"); } catch {}
}

function hapticNotify(type) {
  try { tg && tg.HapticFeedback.notificationOccurred(type); } catch {}
}

function isInsideTelegram() {
  return !!(tg && tg.initData);
}

function initTelegram() {
  if (!tg) return;
  try {
    tg.ready();
    tg.expand();
    if (tg.setHeaderColor) tg.setHeaderColor("#FFFFFF");
    if (tg.setBackgroundColor) tg.setBackgroundColor("#FFFFFF");
    if (typeof tg.disableVerticalSwipes === "function") tg.disableVerticalSwipes();
  } catch {}
  if (tg.BackButton) {
    tg.BackButton.onClick(handleBack);
  }
}

/* ---------------- navigation ---------------- */

function showScreen(name) {
  document.querySelectorAll(".screen").forEach((s) => s.classList.remove("is-active"));
  $("#screen-" + name).classList.add("is-active");
  if (tg && tg.BackButton) {
    if (name === "home") tg.BackButton.hide();
    else tg.BackButton.show();
  }
  window.scrollTo(0, 0);
}

function handleBack() {
  const active = document.querySelector(".screen.is-active");
  if (!active) return;
  if (active.id === "screen-bio") {
    if (state.scanning) return;
    state.scanToken++;
    showScreen("add");
  } else if (active.id === "screen-add") {
    showScreen("home");
  }
}

/* ---------------- home ---------------- */

function cardElement(card) {
  const el = document.createElement("div");
  el.className = "card-preview is-active";

  const blob = document.createElement("div");
  blob.className = "card-blob";

  const inner = document.createElement("div");
  inner.className = "card-inner";

  const top = document.createElement("div");
  top.className = "card-top";
  const logo = document.createElement("img");
  logo.src = "assets/logo.png";
  logo.alt = "";
  logo.className = "logo logo-sm mono";
  const status = document.createElement("span");
  status.className = "card-status " + (card.isActive ? "is-active" : "is-pending");
  const dot = document.createElement("span");
  dot.className = "dot";
  status.append(dot, document.createTextNode(card.isActive ? "Aktiv" : "Gözləmədə"));

  const actions = document.createElement("div");
  actions.className = "card-actions";
  actions.append(status, deleteButton(card.id));
  top.append(logo, actions);

  const number = document.createElement("div");
  number.className = "card-number";
  number.textContent = maskedNumber(card.number);

  const bottom = document.createElement("div");
  bottom.className = "card-bottom";
  const expBlock = document.createElement("div");
  const expLabel = document.createElement("div");
  expLabel.className = "card-label";
  expLabel.textContent = "BİTMƏ";
  const expValue = document.createElement("div");
  expValue.className = "card-expiry";
  expValue.textContent = card.expiry;
  expBlock.append(expLabel, expValue);
  const nfc = document.createElementNS("http://www.w3.org/2000/svg", "svg");
  nfc.setAttribute("class", "icon card-nfc");
  const use = document.createElementNS("http://www.w3.org/2000/svg", "use");
  use.setAttribute("href", "#i-nfc");
  nfc.appendChild(use);
  bottom.append(expBlock, nfc);

  inner.append(top, number, bottom);
  el.append(blob, inner);
  return el;
}

function deleteButton(cardId) {
  const button = document.createElement("button");
  button.type = "button";
  button.className = "card-delete";
  button.setAttribute("aria-label", "Kartı sil");
  const icon = document.createElementNS("http://www.w3.org/2000/svg", "svg");
  icon.setAttribute("class", "icon");
  const use = document.createElementNS("http://www.w3.org/2000/svg", "use");
  use.setAttribute("href", "#i-trash");
  icon.appendChild(use);
  button.appendChild(icon);
  button.addEventListener("click", () => requestDelete(cardId));
  return button;
}

function requestDelete(cardId) {
  const card = state.cards.find((c) => c.id === cardId);
  if (!card) return;
  const message = "«" + maskedNumber(card.number) + "» kartını silmək istəyirsiniz?";
  const apply = (confirmed) => {
    if (confirmed) deleteCard(cardId);
  };

  const telegramConfirm =
    tg &&
    isInsideTelegram() &&
    typeof tg.showConfirm === "function" &&
    typeof tg.isVersionAtLeast === "function" &&
    tg.isVersionAtLeast("6.2");

  if (telegramConfirm) {
    try {
      tg.showConfirm(message, apply);
      return;
    } catch {
      /* fall through to the browser dialog */
    }
  }
  if (window.confirm(message)) deleteCard(cardId);
}

function deleteCard(cardId) {
  state.cards = state.cards.filter((c) => c.id !== cardId);
  saveCards();
  renderHome();
  hapticImpact("medium");
}

function renderHome() {
  const hasCards = state.cards.length > 0;
  $("#home-empty").classList.toggle("hidden", hasCards);
  $("#home-cards").classList.toggle("hidden", !hasCards);

  const list = $("#cards-list");
  list.innerHTML = "";
  state.cards.forEach((card) => list.appendChild(cardElement(card)));
}

/* ---------------- add card ---------------- */

const inpNumber = $("#inp-number");
const inpExpiry = $("#inp-expiry");
const inpCvv = $("#inp-cvv");

function updatePreview() {
  $("#preview-number").textContent = inpNumber.value || "•••• •••• •••• ••••";
  $("#preview-expiry").textContent = inpExpiry.value || "AA/İİ";
  $("#preview-brand").textContent = inpNumber.value ? brandOf(inpNumber.value) : "CARD";
}

function updateValidity() {
  const valid =
    isValidNumber(inpNumber.value) &&
    isValidExpiry(inpExpiry.value) &&
    isValidCVV(inpCvv.value);
  $("#btn-continue").disabled = !valid;
}

function resetForm() {
  inpNumber.value = "";
  inpExpiry.value = "";
  inpCvv.value = "";
  updatePreview();
  updateValidity();
}

function openAddCard() {
  resetForm();
  hapticImpact("light");
  showScreen("add");
}

inpNumber.addEventListener("input", () => {
  inpNumber.value = formatNumber(inpNumber.value);
  updatePreview();
  updateValidity();
});

inpExpiry.addEventListener("input", () => {
  inpExpiry.value = formatExpiry(inpExpiry.value);
  updatePreview();
  updateValidity();
});

inpCvv.addEventListener("input", () => {
  inpCvv.value = sanitizeCVV(inpCvv.value);
  updateValidity();
});

$("#btn-continue").addEventListener("click", () => {
  if ($("#btn-continue").disabled) return;
  state.pending = { number: inpNumber.value, expiry: inpExpiry.value, cvv: inpCvv.value };
  hapticImpact("light");
  startBiometricFlow();
});

/* ---------------- biometric flow ---------------- */

const FACE_COPY = {
  title: "Üz tanınması",
  prompt: "Üzünüzü çərçivəyə yerləşdirin",
  icon: "#i-faceid",
  cta: "Üz tanınmasına başla",
};

function startBiometricFlow() {
  state.scanning = false;
  state.scanToken++;
  showError(null);
  showHint(null);
  applyFaceCopy();
  showScreen("bio");
}

function applyFaceCopy() {
  $("#bio-title").textContent = FACE_COPY.title;
  $("#bio-prompt").textContent = FACE_COPY.prompt;
  $("#bio-icon").querySelector("use").setAttribute("href", FACE_COPY.icon);
  $("#scan-label").textContent = FACE_COPY.cta;
}

function setScanning(on) {
  state.scanning = on;
  $("#scan-spinner").classList.toggle("hidden", !on);
  $("#scan-label").textContent = on ? "Skan edilir..." : FACE_COPY.cta;
  $("#btn-scan").disabled = on;
  $("#btn-bio-back").disabled = on;
}

function showError(message) {
  const el = $("#bio-error");
  if (!message) {
    el.textContent = "";
    el.classList.add("hidden");
  } else {
    el.textContent = message;
    el.classList.remove("hidden");
  }
}

function showHint(message) {
  const el = $("#bio-hint");
  if (!message) {
    el.textContent = "";
    el.classList.add("hidden");
  } else {
    el.textContent = message;
    el.classList.remove("hidden");
  }
}

const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

/* Real Face ID/Touch ID through Telegram's BiometricManager; a simulation
   stands in when the app runs outside Telegram or on an older client. */

function telegramBiometric() {
  const fail = (code, message) => Object.assign(new Error(message), { code });
  return new Promise((resolve, reject) => {
    const bm = tg.BiometricManager;
    const reason = "Kartı aktivləşdirmək üçün üz tanınması";
    let settled = false;
    let watchdog = null;

    const ok = () => {
      if (settled) return;
      settled = true;
      clearTimeout(watchdog);
      resolve();
    };
    const err = (code, message) => {
      if (settled) return;
      settled = true;
      clearTimeout(watchdog);
      reject(fail(code, message));
    };

    // Some clients define BiometricManager but never answer init().
    watchdog = setTimeout(() => err("unavailable", "Biometriya cavab vermədi"), 5000);

    bm.init(() => {
      clearTimeout(watchdog);
      if (settled) return;
      if (!bm.isBiometricAvailable) {
        err("unavailable", "Bu cihazda biometriya mövcud deyil");
        return;
      }
      const authenticate = () => {
        bm.authenticate({ reason }, (success) => {
          if (success) ok();
          else err("failed", "Tanınma uğursuz oldu");
        });
      };
      if (bm.isAccessGranted) {
        authenticate();
      } else {
        bm.requestAccess({ reason }, (granted) => {
          if (granted) authenticate();
          else err("denied", "Biometriya icazəsi verilmədi");
        });
      }
    });
  });
}

function canUseTelegramBiometrics() {
  return !!(
    tg &&
    isInsideTelegram() &&
    tg.BiometricManager &&
    typeof tg.isVersionAtLeast === "function" &&
    tg.isVersionAtLeast("7.2")
  );
}

async function runFaceAuth() {
  const token = state.scanToken;

  if (canUseTelegramBiometrics()) {
    showHint(null);
    try {
      await telegramBiometric();
    } catch (err) {
      if (token !== state.scanToken) return;
      if (err && err.code === "unavailable") {
        // Desktop/web Telegram has no Face ID — keep the demo flowing.
        showHint("Bu cihazda biometriya yoxdur — simulyasiya edilir");
        await delay(1500);
        if (token !== state.scanToken) return;
      } else {
        showError(err && err.message ? err.message : "Tanınma uğursuz oldu");
        hapticNotify("error");
        return;
      }
    }
  } else {
    showHint(isInsideTelegram()
      ? "Telegram versiyanız biometriyanı dəstəkləmir — simulyasiya edilir"
      : "Brauzer rejimi: Face ID yalnız Telegram daxilində işləyir, indi simulyasiya edilir");
    await delay(1500);
  }

  if (token !== state.scanToken) return;
  hapticNotify("success");
  await delay(600);
  if (token !== state.scanToken) return;
  finishFlow();
}

function finishFlow() {
  const pending = state.pending;
  if (!pending) {
    showScreen("home");
    return;
  }
  state.cards.push({
    id: "c_" + Date.now().toString(36) + Math.random().toString(36).slice(2, 6),
    number: pending.number,
    expiry: pending.expiry,
    cvv: pending.cvv,
    isActive: true,
    createdAt: new Date().toISOString(),
  });
  saveCards();
  state.pending = null;
  resetForm();
  renderHome();
  showScreen("home");
}

$("#btn-scan").addEventListener("click", async () => {
  if (state.scanning) return;
  const token = state.scanToken;
  showError(null);
  setScanning(true);
  try {
    await runFaceAuth();
  } finally {
    if (token === state.scanToken) setScanning(false);
  }
});

/* ---------------- wiring ---------------- */

$("#btn-add").addEventListener("click", openAddCard);
$("#btn-add-2").addEventListener("click", openAddCard);
$("#btn-add-back").addEventListener("click", () => showScreen("home"));
$("#btn-bio-back").addEventListener("click", () => {
  if (state.scanning) return;
  state.scanToken++;
  showScreen("add");
});

/* ---------------- boot ---------------- */

initTelegram();
loadCards();
renderHome();
updatePreview();
updateValidity();
