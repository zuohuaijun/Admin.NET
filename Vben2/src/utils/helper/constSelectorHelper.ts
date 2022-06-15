import type { App } from 'vue';
import { useUserStoreWithOut } from '/@/store/modules/user';

export function setupConstSelectorFilter(app: App) {
  // 全局过滤器  在vue文件中调用  $filters.codeToName(code,type)
  app.config.globalProperties.$filters = {
    codeToName(code, type) {
      return codeToName(code, type);
    },
  };
}

//常量值转换
export function codeToName(code, type) {
  const userStore = useUserStoreWithOut();
  try {
    const name = userStore.constSelectorWithOptions
      .filter((x) => x.code === type)
      .map((x) => x.data)
      .map((x) => x.filter((y) => y.code === code))
      .map((x) => x[0].name);
    return name[0];
  } catch (error) {
    return code;
  }
}

export function getSelector(type) {
  const userStore = useUserStoreWithOut();
  const selector = userStore.constSelectorWithOptions.filter((x) => x.code === type)[0].data;
  return selector;
}
