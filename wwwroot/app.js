const API_BASE = window.location.origin; // same host

function saveToken(token){ localStorage.setItem('jwt', token); }
function getToken(){ return localStorage.getItem('jwt'); }

async function api(path, method="GET", body=null){
  const headers = { "Content-Type": "application/json" };
  const token = getToken();
  if(token) headers["Authorization"] = "Bearer " + token;
  const res = await fetch(API_BASE + path, {
    method, headers, body: body ? JSON.stringify(body): null
  });
  const text = await res.text();
  try { return { status: res.status, data: JSON.parse(text) }; }
  catch { return { status: res.status, data: text }; }
}

document.getElementById('btnRegister').onclick = async () => {
  const mobile = document.getElementById('regMobile').value.trim();
  const name = document.getElementById('regName').value.trim();
  const resp = await api('/api/member/register', 'POST', { mobileNumber: mobile, name });
  document.getElementById('registerResult').textContent = JSON.stringify(resp.data, null, 2);
};

document.getElementById('btnVerify').onclick = async () => {
  const mobile = document.getElementById('verMobile').value.trim();
  const otp = document.getElementById('verOtp').value.trim();
  const resp = await api('/api/member/verify', 'POST', { mobileNumber: mobile, otpCode: otp });
  document.getElementById('verifyResult').textContent = JSON.stringify(resp.data, null, 2);
  if(resp.status === 200 && resp.data.token){ saveToken(resp.data.token); alert("JWT saved. You can now call protected APIs."); }
};

document.getElementById('btnAddPoints').onclick = async () => {
  const memberId = parseInt(document.getElementById('addMemberId').value, 10);
  const amount = parseFloat(document.getElementById('addAmount').value);
  const resp = await api('/api/points/add', 'POST', { memberId, purchaseAmount: amount, description: "Web demo" });
  document.getElementById('addResult').textContent = JSON.stringify(resp.data, null, 2);
};

document.getElementById('btnGetPoints').onclick = async () => {
  const memberId = parseInt(document.getElementById('getMemberId').value, 10);
  const resp = await api('/api/points/' + memberId, 'GET');
  document.getElementById('getResult').textContent = JSON.stringify(resp.data, null, 2);
};

document.getElementById('btnRedeem').onclick = async () => {
  const memberId = parseInt(document.getElementById('redMemberId').value, 10);
  const tier = parseInt(document.getElementById('redTier').value, 10);
  const resp = await api('/api/coupons/redeem', 'POST', { memberId, tierPoints: tier });
  document.getElementById('redeemResult').textContent = JSON.stringify(resp.data, null, 2);
};
