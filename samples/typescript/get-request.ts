// Note, crypto is not available in the browser, so this example will not work in the browser. Use nodejs.
import { createHmac } from "crypto";

const BLOXS_API_KEY = "YOUR_API_KEY";
const BLOXS_API_SECRET = "YOUR_API_SECRET";

export function generateAuthHeaders(bodyLength = 0): Record<string, string> {
    const timestamp = Math.round(Date.now() / 1000);
    const tokenParts = `${BLOXS_API_KEY}:${timestamp}:${bodyLength}`;
    const signature = createHmac('sha256', BLOXS_API_SECRET).update(tokenParts).digest('hex');
    return {
        'Authorization': `bloxs ${BLOXS_API_KEY}:${signature}`,
        'X-Timestamp': timestamp.toString(),
    }
}

export async function getRequest<T>(url: string): Promise<T> {
    const headers = generateAuthHeaders();
    const response = await fetch(url, { headers });
    const result = await response.json();
    return result as T;
}