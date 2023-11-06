export default defineEventHandler(async (event) => {

    const config = useRuntimeConfig()

    const guid = getRouterParam(event, 'guid')
    const body = await readBody(event)

    const uri = `${config.apiBaseUrl}/rfq/${guid}`

    const data = await $fetch(uri, {
        method: 'POST',
        headers: {
            Authorization: `Basic ${btoa(config.apiUser + ':' + config.apiPass)}`
        },
        body: body
    })
        
    return data
})