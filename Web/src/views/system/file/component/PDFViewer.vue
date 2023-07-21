<template>
	<div id="viewerContainer">
		<div id="viewer" class="pdfViewer"></div>
	</div>
</template>

<script lang="ts" setup>
import { onMounted } from 'vue';
import * as pdfjsLib from 'pdfjs-dist';
import * as pdfjsViewer from 'pdfjs-dist/web/pdf_viewer';

const props = defineProps({
	pdfUrl: String,
});

onMounted(async () => {
	if (props.pdfUrl == undefined) return;

	pdfjsLib.GlobalWorkerOptions.workerSrc = '/pdf.worker.min.js';

	const container = document.getElementById('viewerContainer') as HTMLDivElement;
	const pdfViewer = new pdfjsViewer.PDFViewer({
		container,
		eventBus: new pdfjsViewer.EventBus(),
		useOnlyCssZoom: true,
	});

	const loadingTask = pdfjsLib.getDocument(props.pdfUrl);
	const pdfDoc = await loadingTask.promise;
	pdfViewer.setDocument(pdfDoc);
});
</script>

<style>
/* 根据需要添加样式 */
</style>
