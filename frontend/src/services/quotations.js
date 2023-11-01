import axios from "axios"

const username = import.meta.env.VITE_APIUSER
const password = import.meta.env.VITE_APIPASS

// Codifique as credenciais para Base64
const base64Credentials = btoa(`${username}:${password}`);

// Configuração da solicitação com autenticação básica
const config = {
    headers: {
        'Authorization': `Basic ${base64Credentials}`,
    },
};

const instance = axios.create({
    baseURL: import.meta.env.VITE_APIHOST
})

export const getQuotation = (guid) => instance.get(`/rfq/${guid}`, config)

export const postQuotation = (guid, document) => instance.post(`/rfq/${guid}`, document, config)